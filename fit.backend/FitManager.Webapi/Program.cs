using FitManager.Application.Infrastructure;
using FitManager.Application.Services;
using FitManager.Webapi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Authentication ******************************************************************************
        // For vue dev server
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.OnAppendCookie = cookieContext =>
            {
                cookieContext.CookieOptions.Secure = true;
                cookieContext.CookieOptions.SameSite = builder.Environment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Strict;
            };
        });
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                o.LoginPath = "/account/signin";
                o.AccessDeniedPath = "/account/signin";
            });
        // Protect static files (= the SPA)
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVueDevserver",
                    builder => builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                        .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
        }
        // *************************************************************************************************

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //options.JsonSerializerOptions.WriteIndented = true;
        });

        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(typeof(FitManager.Application.Dto.MappingProfile));
        builder.Services.AddTransient<PackageEventService>();
        builder.Services.AddTransient<CompanyService>();
        builder.Services.AddTransient(opt => new AzureAdClient(
            tenantId: builder.Configuration["AzureAd:TenantId"],
            clientId: builder.Configuration["AzureAd:ClientId"],
            clientSecret: builder.Configuration["AzureAd:ClientSecret"]));
        builder.Services.AddDbContext<FitContext>(opt =>
        {
            opt.UseSqlServer(
                builder.Configuration.GetConnectionString("SqlServer"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
        });

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVueDevserver",
                    builder => builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                        .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
        }

        // APP *************************************************************************************************
        var app = builder.Build();
        var publicFileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot"));
        var adminFileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "admin"));

        using (var scope = app.Services.CreateScope())
        {
            using (var db = scope.ServiceProvider.GetRequiredService<FitContext>())
            {
                db.CreateDatabase(isDevelopment: app.Environment.IsDevelopment());
            }
        }

        // FallbackToFile for SPA client side routing.
        app.Use(async (context, next) =>
        {
            // Is this a request for a controller? No modification, process pipeline.
            if (context.GetEndpoint() != null) { await next(); return; }
            // Send 404 if an api controller is not found (not serve the spa).
            if (context.Request.Path.StartsWithSegments("/api")) { await next(); return; }
            // Send 404 if account controller is not found (not serve the spa).
            if (context.Request.Path.StartsWithSegments("/account")) { await next(); return; }
            // Process request pipeline with /admin/index.html if a request with /admin does not match a file.
            if (context.Request.Path.StartsWithSegments("/admin", out var remaining))
            {
                if (!adminFileProvider.GetFileInfo(remaining).Exists)
                    context.Request.Path = "/admin/index.html";
            }
            // Process request pipeline with /index.html if a request does not match a file.
            else if (!publicFileProvider.GetFileInfo(context.Request.Path).Exists)
            {
                context.Request.Path = "/index.html";
            }
            await next();
            return;
        });

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["SyncfusionKey"]);
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("AllowVueDevserver");
        }
        app.UseCookiePolicy();
        // Map public SPA before authentication.
        app.UseStaticFiles(new StaticFileOptions { FileProvider = publicFileProvider });
        app.UseAuthentication();
        app.UseAuthorization();
        // Map admin SPA after authentication.
        app.UseStaticFiles(new StaticFileOptions { FileProvider = adminFileProvider, RequestPath = "/admin" });
        app.MapControllers();
        app.Run();
    }
}
using FitManager.Application.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// JWT Authentication ******************************************************************************
byte[] secret = Convert.FromBase64String(builder.Configuration["JwtSecret"]);
builder.Services
    .AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });
// *************************************************************************************************

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(FitManager.Application.Dto.MappingProfile));
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
        options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
    });
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        using (var db = scope.ServiceProvider.GetRequiredService<FitContext>())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
        }
    }
    app.UseCors();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.Run();

using BeamerProtector.Application.Infrastructure;
using BeamerProtector.Webapp.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BeamerProtectorContext>(opt => opt
    .UseSqlite(builder.Configuration.GetConnectionString("Default")));

// AZURE AD LOGIN for Authorization in ASP.NET Core. Not used yet.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllers();
builder.Services.AddRazorPages(opt => opt.Conventions.AuthorizeFolder("/"))
    .AddMicrosoftIdentityUI();
builder.Services.AddScoped(opt => new AzureAdClient(
    tenantId: builder.Configuration["AzureAd:TenantId"],
    clientId: builder.Configuration["AzureAd:ClientId"],
    redirectUrl: builder.Configuration["AzureAd:RedirectUrl"],
    clientSecret: builder.Configuration["AzureAd:ClientSecret"],
    scope: builder.Configuration["AzureAd:Scope"]));
builder.Services.AddHostedService<MonitorService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    using (var db = scope.ServiceProvider.GetRequiredService<BeamerProtectorContext>())
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.Seed();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.Run();
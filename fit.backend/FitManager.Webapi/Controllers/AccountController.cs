using FitManager.Application.Infrastructure;
using FitManager.Webapi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeamerProtector.Webapp.Controllers
{
    /// <summary>
    /// Controller to handle OAuth2 Sign-in for the mailer deamon account.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly AzureAdClient _adClient;
        private readonly FitContext _db;
        private readonly byte[] _key;                // To encrypt the refresh token
        private string AuthorizeUserUrl => $"https://{HttpContext.Request.Host}/account/authorize";
        private string AuthorizeMailUrl => $"https://{HttpContext.Request.Host}/account/authorizemail";

        public AccountController(AzureAdClient adClient, FitContext db, IConfiguration _config)
        {
            _adClient = adClient;
            _db = db;
            _key = Convert.FromBase64String(_config["TokenKey"] ?? throw new ApplicationException("Token Key is not set."));
        }

        /// <summary>
        /// Route for authorization fallback. ASP redirects to /signin in authenticaton middleware.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signin")]
        public IActionResult Signin()
        {
            return Redirect(_adClient.GetLoginUrl(AuthorizeUserUrl, "user.read"));
        }

        /// <summary>
        /// Process OAuth2 flow for user login in admin section.
        /// </summary>
        /// <returns></returns>
        [HttpGet("authorize")]
        public async Task<IActionResult> AuthorizeAccount([FromQuery] string code)
        {
            var (authToken, refreshToken) = await _adClient.GetToken(code, AuthorizeUserUrl, "user.read");
            var graph = _adClient.GetGraphServiceClientFromToken(authToken);
            var me = await graph.Me.Request().GetAsync();
            // Todo: Filter special users. Otherwise send Unauthorized().
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, me.UserPrincipalName),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
                //IsPersistent = true
            };
            await HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("/admin");
        }

        /// <summary>
        /// GET /account/signinMailaccount
        /// Entrypoint for configuring mailaccount.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signinMailaccount")]
        public IActionResult SigninMailaccount()
        {
            return Redirect(_adClient.GetLoginUrl(AuthorizeMailUrl, "user.read offline_access mail.send"));
        }

        /// <summary>
        /// Callback for SigninMailaccount. Route has to be corresponding with callback URL in
        /// appsettings.json and in the Azure App Registration.
        /// </summary>
        [HttpGet("authorizemail")]
        public async Task<IActionResult> AuthorizeMailAccount([FromQuery] string code)
        {
            var (authToken, refreshToken) = await _adClient.GetToken(code, AuthorizeMailUrl, "user.read offline_access mail.send");
            if (refreshToken is null) { return BadRequest("No refresh token in payload."); }
            var graph = _adClient.GetGraphServiceClientFromToken(authToken);
            var me = await graph.Me.Request().GetAsync();
            var message = new Message
            {
                Subject = "Account registriert",
                Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = "Du hast deinen Account zum Senden von Statusmeldungen erfolgreich registriert."
                },
                ToRecipients = new List<Recipient>()
                {
                    new Recipient { EmailAddress = new EmailAddress { Address = me.Mail } }
                }
            };
            await graph.Me
                .SendMail(message, SaveToSentItems: false)
                .Request()
                .PostAsync();

            await _db.SetMailerAccount(me.Mail, refreshToken, _key);
            var mailer = await _db.GetMailerAccount(_key);
            if (refreshToken != mailer.refreshToken)
            {
                Debug.WriteLine("Wrong encryption");
            }
            return Redirect("/admin");
        }

        [HttpGet("sendMail/{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> SendMail(Guid guid)
        {
            var (accountName, refreshToken) = await _db.GetMailerAccount(_key);
            if (refreshToken is null) { return BadRequest("No refresh token in payload."); }
            var (authToken, newRefreshToken) = await _adClient.GetNewToken(refreshToken, "user.read offline_access mail.send");
            var graph = _adClient.GetGraphServiceClientFromToken(authToken);
            var me = await graph.Me.Request().GetAsync();

            var company = await _db.Companies.Include(a => a.ContactPartners).FirstAsync(a => a.Guid == guid);
            var partners = company.ContactPartners.ToList();
            var recipients = new List<Recipient>();
            foreach(var p in partners)
            {
                recipients.Add(new Recipient { EmailAddress = new EmailAddress { Address = p.Email } });
            }

            var message = new Message
            {
                Subject = "Firma registriert",
                Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = $"Sie haben sich erfolgreich beim Firmeninformationstag registriert. Hier https://fit-ohiliroc6ha2yrc3.azurewebsites.net/companypage/{guid} kommen Sie zum Firmenportal"
                },
                ToRecipients = recipients //new List<Recipient>()
                //{
                    //new Recipient { EmailAddress = new EmailAddress { Address = me.Mail } }
                //}
            };
            await graph.Me
                .SendMail(message, SaveToSentItems: false)
                .Request()
                .PostAsync();
            return Ok();
        }

        /// <summary>
        /// Signout if a user wants to deregister the mailaccount.
        /// </summary>
        [HttpGet("signoutMailaccount")]
        [Authorize]
        public async Task<IActionResult> SignoutMailaccount()
        {
            var config = await _db.GetConfig();
            config.MailerAccountname = null;
            config.MailerRefreshToken = null;
            await _db.SetConfig(config);
            return Redirect("https://login.microsoftonline.com/logout.srf");
        }

        /// <summary>
        /// Signout for admin users. Put a link to /signout in your spa.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signout")]
        [Authorize]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("https://login.microsoftonline.com/logout.srf");
        }

        /// <summary>
        /// GET account/accountinfo
        /// Send the data of the submitted cookie to the client.
        /// </summary>
        [HttpGet("accountinfo")]
        [Authorize]
        public IActionResult GetAccountinfo()
        {
            var authenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (!authenticated) { return Unauthorized(); }
            return Ok(new
            {
                Username = HttpContext.User.Identity?.Name
            });
        }
    }
}
using FitManager.Application.Infrastructure;
using FitManager.Webapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BeamerProtector.Webapp.Controllers
{
    /// <summary>
    /// Controller to handle OAuth2 Sign-in for the mailer deamon account.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AzureAdClient _adClient;
        private readonly FitContext _db;
        private readonly byte[] _key;

        public AccountController(AzureAdClient adClient, FitContext db, IConfiguration _config)
        {
            _adClient = adClient;
            _db = db;
            _key = Convert.FromBase64String(_config["TokenKey"] ?? throw new ApplicationException("Token Key is not set."));
        }

        /// <summary>
        /// GET /account/signinMailaccount
        /// Geht zu login.microsoft.com und startet den OAuth2 Workflow.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signinMailaccount")]
        public IActionResult SigninMailaccount()
        {
            return Redirect(_adClient.LoginUrl);
        }

        [HttpGet("signoutMailaccount")]
        public async Task<IActionResult> SignoutMailaccount()
        {
            var config = await _db.GetConfig();
            config.MailerAccountname = null;
            config.MailerRefreshToken = null;
            await _db.SetConfig(config);
            return Redirect("/");
        }

        /// <summary>
        /// Callback for SigninMailaccount. Route has to be corresponding with callback URL in
        /// appsettings.json and in the Azure App Registration.
        /// </summary>
        [HttpGet("authorizeMailAccount")]
        public async Task<IActionResult> AuthorizeMailAccount([FromQuery] string code)
        {
            var (authToken, refreshToken) = await _adClient.GetToken(code);
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
            return Redirect("/");
        }
    }
}

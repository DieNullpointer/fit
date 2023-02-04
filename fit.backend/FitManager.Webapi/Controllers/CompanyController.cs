using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly FitContext _db;

        private readonly IConfiguration _config;
        public record PartnerDto(string title, string firstname, string lastname, string email, string telNr, string function, string? mobilNr = null, bool mainPartner = false);
        public record RegisterDto(string name, string address, string country, string plz, string place, string billAddress, List<PartnerDto> partners, string package, string eventName);
        public CompanyController(FitContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        //  api/company
        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = _db.Companies.Include(a => a.Package).Include(a => a.Event).OrderBy(c => c.Name).ToList();
            var export = companies.Select(p => new
            {
                p.Guid,
                p.Name,
                p.Address,
                p.Country,
                p.Plz,
                p.Place,
                p.BillAddress,
                EventName = p.Event.Name
            });
            return Ok(export);
        }

        //  api/company/{guid}
        [HttpGet("{guid:Guid}")]
        public IActionResult GetCompany(Guid guid)
        {
            var partners = _db.ContactPartners.Where(a => a.Company.Guid== guid).ToList();
            return partners is null ? BadRequest() : Ok(partners);
        }

        //  api/company/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var package = _db.Packages.Where(a => a.Name == register.package).First();
            var events = _db.Events.Where(a => a.Name == register.eventName).First();
            var company = new Company(register.name, register.address, register.country, register.plz, register.place, register.billAddress, package, events);
            foreach(var i in register.partners)
            {
                _db.ContactPartners.Add(new ContactPartner(i.title, i.firstname, i.lastname, i.email, i.telNr, i.function, company, i.mobilNr, i.mainPartner));
            }
            _db.Companies.Add(company);
            _db.SaveChanges();

            foreach (var i in register.partners)
            {
                //SpgMailClient client = new SpgMailClient(new MailKit.Net.Smtp.SmtpClient());
                var searchuser = _config["Searchuser"];
                var searchpass = _config["Searchpass"];
                var a = await SpgMailClient.Create(searchuser, searchpass);
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Fit-Team", searchuser + "@spengergasse.at"));
                message.To.Add(new MailboxAddress($"{i.firstname + i.lastname}", i.email));
                message.Subject = "FIT TEST MAIL";
                //message.Body = new TextPart("plain")
                //{
                //    Text = @"Diese Mail wird von ASP geschicktMoin Basti!"
                //};
                var builder = new BodyBuilder();
                builder.HtmlBody = string.Format(@"<p>Diese Mail wird zur Bestätigung der Anmeldung vom " + register.eventName + " versendet.</p><p>Wir freuen uns auf euch.</p><br><p>Mit freundlichen Grüßen</p><p>Das Fit-Team.</p>");
                message.Body = builder.ToMessageBody();
                await a.SendMailAsync(message);
            }
            return Ok(company);
        }
    }
}

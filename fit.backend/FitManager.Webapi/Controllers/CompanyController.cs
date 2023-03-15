using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IMapper _mapper;

        public CompanyController(FitContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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

        [HttpDelete("delete/{guid:Guid}")]
        public async Task<IActionResult> DeleteCompany(Guid guid)
        {
            var company = await _db.Companies.FirstAsync(a => a.Guid == guid);
            if(company is null)
                return BadRequest();
            _db.Companies.Remove(company);
            return Ok(company);
        }

        //  api/company/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var company = _mapper.Map<Company>(register, opt => opt.AfterMap(async(dto, entity) =>
            {
                entity.Event = await _db.Events.FirstAsync(a => a.Guid == register.eventGuid);
                entity.Package = await _db.Packages.FirstAsync(a => a.Guid == register.packageGuid);
            }));
            //var package = await _db.Packages.FirstAsync(a => a.Name == register.package);
            //var events = await _db.Events.FirstAsync(a => a.Name == register.eventName);
            //var company = new Company(register.name, register.address, register.country, register.plz, register.place, register.billAddress, package, events);
            foreach(var i in register.partners)
            {
                await _db.ContactPartners.AddAsync(new ContactPartner(i.title, i.firstname, i.lastname, i.email, i.telNr, i.function, company, i.mobilNr, i.mainPartner));
            }
            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();

            /* foreach (var i in register.partners)
            {
                //SpgMailClient client = new SpgMailClient(new MailKit.Net.Smtp.SmtpClient());
                var searchuser = _config["Searchuser"];
                var searchpass = _config["Searchpass"];
                var a = await SpgMailClient.Create(searchuser, searchpass);
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Fit-Team", $"{searchuser}@spengergasse.at"));
                message.To.Add(new MailboxAddress($"{i.firstname} {i.lastname}", i.email));
                message.Subject = "FIT TEST MAIL";
                //message.Body = new TextPart("plain")
                //{
                //    Text = @"Diese Mail wird von ASP geschicktMoin Basti!"
                //};
                var builder = new BodyBuilder();
                builder.HtmlBody = string.Format(@"<p>Diese Mail wird zur Bestätigung der Anmeldung vom " + register.eventName + " versendet.</p><p>Wir freuen uns auf euch.</p><br><p>Mit freundlichen Grüßen</p><p>Das Fit-Team.</p>");
                message.Body = builder.ToMessageBody();
                await a.SendMailAsync(message);
            } */
            return Ok(company);
        }
    }
}

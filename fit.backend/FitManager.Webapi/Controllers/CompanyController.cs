using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly FitContext _db;

        public record PartnerDto(string title, string firstname, string lastname, string email, string telNr, string function, string? mobilNr = null, bool mainPartner = false);
        public record RegisterDto(string name, string address, string country, string plz, string place, string billAddress, List<PartnerDto> partners, string package, string eventName);
        public CompanyController(FitContext db)
        {
            _db = db;
        }

        //  api/company
        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = _db.Companies.Include(a => a.Package).OrderBy(c => c.Name).ToList();
            var export = companies.Select(p => new
            {
                p.Guid,
                p.Name,
                p.Address,
                p.Country,
                p.Plz,
                p.Place,
                p.BillAddress,
                EventName = p.Event.Name,
                package = new
                {
                    p.Package.Name,
                    p.Package.Price
                }
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
        public IActionResult Register([FromBody] RegisterDto register)
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
            return Ok(company);
        }
    }
}

using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
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
        public record RegisterDto(string name, string address, string country, string plz, string billAddress, List<PartnerDto> partners);
        public CompanyController(FitContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = _db.Companies.OrderBy(c => c.Name).ToList();
            return Ok(companies);
        }

        [HttpGet("{guid:Guid}")]
        public IActionResult GetCompany(Guid guid)
        {
            var companies = _db.ContactPartners.Where(a => a.Company.Guid== guid).ToList();
            return Ok(companies);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto register)
        {
            var company = new Company(register.name, register.address, register.country, register.plz, register.billAddress);
            foreach(var i in register.partners)
            {
                _db.ContactPartners.Add(new ContactPartner(i.title, i.firstname, i.lastname, i.email, i.telNr, i.function, company, i.mobilNr, i.mainPartner));
            }
            _db.Companies.Add(company);
            _db.SaveChanges();
            return Ok(register);
        }
    }
}

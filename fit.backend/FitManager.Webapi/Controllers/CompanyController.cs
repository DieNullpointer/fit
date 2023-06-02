using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using FitManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CompanyController : ControllerBase
    {
        private readonly FitContext _db;
        private readonly IMapper _mapper;
        private readonly CompanyService _service;

        public CompanyController(FitContext db, IMapper mapper, CompanyService service)
        {
            _db = db;
            _mapper = mapper;
            _service = service;
        }

        //  api/company
        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = _db.Companies.Include(a => a.Package).Include(a => a.Event).Include(a => a.ContactPartners).OrderBy(c => c.Name).ToList();
            var export = companies.Select(p => new
            {
                p.Guid,
                p.Name,
                p.Address,
                p.Country,
                p.Plz,
                p.Place,
                p.BillAddress,
                p.Description,
                p.HasPaid,
                Event = new { p.Event.Guid, p.Event.Name},
                Package = new {p.Package.Guid, p.Package.Name},
                partners = p.ContactPartners.Select(d => new
                {
                    d.Guid,
                    d.Title,
                    d.Firstname,
                    d.Lastname,
                    d.Email,
                    d.TelNr,
                    d.MobilNr,
                    d.Function,
                    d.MainPartner
                })
            });
            return Ok(export);
        }

        //  api/company/{guid}
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> GetCompany(Guid guid)
        {
            var company = await _db.Companies.Include(a => a.Event).Include(a => a.Package).Include(c => c.ContactPartners).FirstAsync(c => c.Guid == guid);
            if (company is null) return BadRequest();
            return Ok(company);
        }

        // api/company/delete
        [HttpDelete("delete/{guid:Guid}")]
        public async Task<IActionResult> DeleteCompany(Guid guid)
        {
            try
            {
                if (await _service.DeleteCompany(guid))
                    return Ok();
                return BadRequest();
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        //  api/company/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CompanyCmd register)
        {
            try
            {
                return Ok(await _service.AddCompany(register));
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPost("addinserat/{guid:Guid}")]
        public async Task<IActionResult> AddInserat([FromForm] IFormFile formFile, Guid guid)
        {
            if(!(await _db.Companies.Include(a => a.Package).FirstAsync(a => a.Guid == guid)).Package.Name.ToLower().Contains("inserat"))
                return BadRequest("Firma hat kein Package das ein Inserat erlaubt");
            if (formFile.ContentType != "application/pdf")
                return BadRequest("Es werden nur PDF Dokumente akzeptiert");
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{guid}");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, $"Inserat-{guid}.{formFile.FileName.Split(".").Last()}");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return Ok(new { FileName = $"Inserat-{guid}", formFile.Length });
        }

        [HttpPost("addlogo/{guid:Guid}")]
        public async Task<IActionResult> AddLogo([FromForm] IFormFile formFile, Guid guid)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{guid}");
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, $"Logo-{guid}.{formFile.FileName.Split(".").Last()}");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return Ok(new { FileName = $"Logo-{guid}", formFile.Length });
        }

        [HttpPost("adddescription")]
        public async Task<IActionResult> AddDescription([FromBody] AddDescriptionDto descriptionDto)
        {
            try
            {
                return Ok(await _service.AddDescription(descriptionDto.description, descriptionDto.guid));
            }
            catch(ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPut("change")]
        public async Task<IActionResult> EditCompany([FromBody] CompanyDto company)
        {
            try
            {
                return Ok(await _service.EditCompany(company));
            }
            catch(ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> NewEvent([FromBody] SignInCmd sign)
        {
            var company = await _db.Companies.Include(a => a.Package).Include(a => a.Event).FirstAsync(a => a.Guid == sign.guid);
            var events = await _db.Events.FirstAsync(a => a.Guid == sign.guid);
            if (events.Date < DateTime.Now)
                return BadRequest("Keine Vergangenen Events zur Anmeldung möglich");
            var package = await _db.Packages.FirstAsync(a => a.Guid == sign.guid);
            company.LastPackage = company.Package;
            company.Event = events;
            company.Package = package;
            return Ok(company.Guid);
        }
    }
}

﻿using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using FitManager.Application.Services;
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
        public async Task<IActionResult> GetCompany(Guid guid)
        {
            var partners = await _db.ContactPartners.Where(a => a.Company.Guid== guid).ToListAsync();
            return partners is null ? BadRequest() : Ok(partners);
        }

        [HttpDelete("delete/{guid:Guid}")]
        public async Task<IActionResult> DeleteCompany(Guid guid)
        {
            try
            {
                if (await _service.DeleteCompany(guid))
                    return Ok();
                return BadRequest();
            }
            catch(ServiceException e) { return BadRequest(e.Message); }

            /*var company = await _db.Companies.FirstAsync(a => a.Guid == guid);
            if(company is null)
                return BadRequest();
            _db.Companies.Remove(company);
            return Ok(company);*/
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
    }
}

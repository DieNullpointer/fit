using FitManager.Application.Infrastructure;
using FitManager.Application.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using FitManager.Application.Services;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PackageController : ControllerBase
    {
        private readonly PackageEventService _service;
        private readonly FitContext _db;

        public PackageController(FitContext db, PackageEventService service)
        {
            _service = service;
            _db = db;
        }

        // api/package
        [HttpGet]
        public IActionResult AllPackages()
        {
            var p = _service.Packages.ToList();
            if(p is null)
                return BadRequest();
            var export = p.Select(a => new
            {
                a.Guid,
                a.Name,
                a.Price
            });
            return Ok(export);
        }

        // api/package/add
        [HttpPost("add")]
        public async Task<IActionResult> CreatePackage([FromBody] PackageCmd package)
        {
            try
            {
                return Ok(await _service.AddPackage(package));
            }
            catch(ServiceException e) { return BadRequest(e.Message); }
        }
    }
}

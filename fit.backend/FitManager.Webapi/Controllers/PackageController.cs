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

        public PackageController(FitContext db, PackageEventService service)
        {
            _service = service;
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

        public record ChangePackageDto(Guid id, string newName, string newPrice);

    
        //api/package/change
        [HttpPut("change")]
        public async Task<IActionResult> ChangePackage([FromBody] ChangePackageDto change){
            var packages = _db.Packages.FirstOrDefault(p => p.Guid == change.id);

            if (packages == null)
            {
                return BadRequest("Package does not exist");
            }

            packages.Name = change.newName ?? packages.Name;
            packages.Price = decimal.Parse(change.newPrice);

            await _db.SaveChangesAsync();

            return Ok(packages);
        }

        [HttpPost("delete/{id:Guid}")]
        public async Task<IActionResult> DeletePackage (Guid id) { 
            var packages = await _db.Packages.FirstAsync(a => a.Guid == id);
            
            if (packages == null)
                throw new ServiceException("Package doesnt exist");

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return Ok();
        }
    }
}

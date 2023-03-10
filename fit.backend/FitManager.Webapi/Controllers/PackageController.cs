using FitManager.Application.Infrastructure;
using FitManager.Application.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly FitContext _db;

        public PackageController(FitContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult AllPackages()
        {
            var p = _db.Packages.ToList();
            if(p is null)
                return BadRequest();
            var export = p.Select(a => new
            {
                a.Name,
                a.Guid
            });
            return Ok(export);
        }

        [HttpPost("add")]
        public IActionResult CreatePackage([FromBody] PackageDto package)
        {
            if(_db.Packages.Where(a => a.Name == package.name).Count() > 0)
                return BadRequest("gibt es schon");
            try
            {
                _db.Packages.Add(new Application.Model.Package(package.name, decimal.Parse(package.price)));
                _db.SaveChanges();
            }
            catch
            {
                return BadRequest("Price ist keine Zahl");
            }
            return Ok();
        }
    }
}

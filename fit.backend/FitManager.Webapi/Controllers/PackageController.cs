using FitManager.Application.Infrastructure;
using FitManager.Application.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using FitManager.Application.Services;
using System.Threading.Tasks;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly FitContext _db;
        private readonly PackageEventService _service;

        public PackageController(FitContext db, PackageEventService service)
        {
            _db = db;
            _service = service;
        }

        // api/package
        [HttpGet]
        public IActionResult AllPackages()
        {
            var p = _db.Packages.ToList();
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

        [HttpPost("change")]
        public async Task<IActionResult> ChangePackage([FromBody] ChangePackageDto change){
            var packages = _db.Packages.FirstOrDefault(p => p.Guid == change.id);

            if (packages == null)
            {
                return BadRequest("Package does not exist");
            }

            packages.Name = change.newName ?? packages.Name;
            packages.Price = change.price ?? packages.Price;

            await _db.SaveChangesAsync();

            return Ok(packages);
        }

        public record DeletePackageDto(Guid id);

        [HttpPost("delete")];
        public async Task<IActionResult> DeletePackage ([FromBody] DeletePackageDto delete){
            var packages = await _db.Package.Include(p=>p.package).FirstAsync(a => a.Guid == guid);
            
            if (packages == null)
                throw new ServiceException("Package doesnt exist");

            _db.Package.RemoveRange(packages.Package.ToList());

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return true;
        }
    }
}

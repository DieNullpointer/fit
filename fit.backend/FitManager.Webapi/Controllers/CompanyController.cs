using FitManager.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly FitContext _db;

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
    }
}

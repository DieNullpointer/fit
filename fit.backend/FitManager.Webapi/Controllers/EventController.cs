using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using FitManager.Application.Dto;
using FitManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly FitContext _db;

        public EventController(FitContext db)
        {
            _db = db;
        }

        //  api/event
        [HttpGet]
        public IActionResult GetAllEvents()
        {
            
        }

        //  api/event/now
        [HttpGet("now")]
        public IActionResult GetCurrentEvent()
        {
            var events = _db.Events.Include(a => a.Companies).Where(a => DateTime.UtcNow.Date <= a.Date).OrderBy(a => a.Date).First();

            //var settings = Path.Combine("appsettings.Development.json");
            //var config = System.Text.Json.JsonDocument.Parse(System.IO.File.ReadAllText(settings)).RootElement;
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(config.GetProperty("SyncfusionKey").GetString());

            //PdfGenerator pdf = new PdfGenerator();
            //var b = pdf.GenerateInvoice();
            //return File(b, "application/pdf", "your_filename.pdf");
            return events is null ? BadRequest("No event is planned") : Ok(events);
        }

        //  api/event/{name}
        [HttpGet("{name}")]
        public IActionResult GetEventByName(string name)
        {
            var events = _db.Events.Where(e => e.Name.ToLower() == name.ToLower()).First();
            return events is null ? BadRequest("Event does not exist") : Ok(events);
        }

        //  api/event/add
        [HttpPost("add")]
        public IActionResult AddEvent([FromBody] EventCmd events)
        {
            if(_db.Events.Where(a => a.Name == events.name).Any())
                return BadRequest("Event already exists");
            
            _db.Events.Add(ev);
            _db.SaveChanges();
            return Ok();
        }
    }
}

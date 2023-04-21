using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using FitManager.Application.Dto;
using FitManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly FitContext _db;
        private readonly PackageEventService _service;

        public EventController(FitContext db, PackageEventService service)
        {
            _db = db;
            _service = service;
        }

        //  api/event
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _db.Events.Where(a => a.Date > DateTime.UtcNow.Date).OrderBy(a => a.Date).ToListAsync();
            if (events is null)
                return BadRequest();
            var export = events.Select(a => new
            {
                a.Guid,
                a.Name,
                Date = a.Date.ToString("dd/MM/yyyy")
            });
            return Ok(export);
        }

        //  api/event/now
        [HttpGet("now")]
        public IActionResult GetCurrentEvent()
        {
            var events = _db.Events.Include(a => a.Companies).Where(a => DateTime.UtcNow.Date <= a.Date).OrderBy(a => a.Date).First();
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
        public async Task<IActionResult> AddEvent([FromBody] EventCmd events)
        {
            try
            {
                return Ok(await _service.AddEvent(events));
            }
            catch(ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangeEvent(string name, string newName, DateTime date)
            {
                var events = _db.Events.FirstOrDefault(e => e.name.ToLower() == name.ToLower());

                if (events == null)
                {
                    return BadRequest("Event does not exist");
                }

                events.name = newName ?? events.name; // use newName if it's not null, otherwise keep the current value of events.name
                events.date = date ?? events.date;

                await _db.SaveChangesAsync();

                return Ok(events);
            }
    }
}

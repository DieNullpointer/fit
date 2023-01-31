using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly FitContext _db;

        public record AddEventDto(string name, DateTime date);

        public EventController(FitContext db)
        {
            _db = db;
        }

        //  api/event
        [HttpGet]
        public IActionResult GetAllEvents()
        {
            var events = _db.Events.OrderBy(a => a.Name).ToList();
            return events is null ? BadRequest() : Ok(events);
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
        public IActionResult AddEvent([FromBody] AddEventDto events)
        {
            if(_db.Events.Where(a => a.Name == events.name).Any())
                return BadRequest("Event already exists");
            var ev = new Event(name: events.name, date: events.date);
            _db.Events.Add(ev);
            _db.SaveChanges();
            return Ok();
        }
    }
}

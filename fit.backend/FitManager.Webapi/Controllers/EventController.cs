using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using FitManager.Application.Dto;
using FitManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EventController : ControllerBase
    {
        private readonly PackageEventService _service;

        public EventController(FitContext db, PackageEventService service)
        {
            _service = service;
        }

        //  api/event
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _service.Events.Include(a => a.Packages).Where(a => a.Date > DateTime.UtcNow.Date).OrderBy(a => a.Date).ToListAsync();
            if (events is null)
                return BadRequest();

            var export = events.Select(a => new
            {
                a.Guid,
                a.Name,
                Date = a.Date.ToString("dd/MM/yyyy"),
                Packages = a.Packages.Select(b => new
                {
                    b.Guid,
                    b.Name,
                    b.Price
                })
            });
            return Ok(export);
        }

        //  api/event/now
        [HttpGet("now")]
        public async Task<IActionResult> GetCurrentEvent()
        {
            var events = await _service.Events.Include(a => a.Companies).Where(a => DateTime.UtcNow.Date <= a.Date).OrderBy(a => a.Date).FirstAsync();
            return events is null ? BadRequest("Kein Event in Planung") : Ok(events);
        }

        //  api/event/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var events = await _service.Events.Where(e => e.Name.ToLower() == name.ToLower()).FirstAsync();
            return events is null ? BadRequest("Event existiert nicht") : Ok(events);
        }

        //  api/event/assign
        [HttpPut("assign")]
        public async Task<IActionResult> AssignPackages([FromBody] AssignPackageCmd packages)
        {
            try
            {
                if (await _service.AssignPackages(packages))
                    return Ok();
                return BadRequest();
            }
            catch(ServiceException e) { return BadRequest(e.Message); }
        }

        //  api/event/delete
        [HttpDelete("delete/{guid:Guid}")]
        public async Task<IActionResult> DeleteEvent(Guid guid)
        {
            try
            {
                if (await _service.DeleteEvent(guid)) return Ok();
                return BadRequest();
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        //  api/event/add
        [HttpPost("add")]
        public async Task<IActionResult> AddEvent([FromBody] EventCmd events)
        {
            try
            {
                return Ok(await _service.AddEvent(events));
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }
    }
}
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PartnerController : ControllerBase
    {
        private readonly CompanyService _service;
        private readonly FitContext _db;

        public PartnerController(FitContext db, CompanyService service)
        {
            _db = db;
            _service = service;
        }

        [HttpPut("change")]
        public async Task<IActionResult> ChangeContact([FromBody] ChangePartner changeContact)
        {
            var contact = await _db.ContactPartners.FirstAsync(a => a.Guid == changeContact.Guid);
            if (contact is null)
                    return NotFound("Ansprechpartner nicht gefunden");

            if (changeContact.MainPartner && await _db.ContactPartners.FirstAsync(a => a.MainPartner) is not null)
                return BadRequest("Es gibt bereits einen Hauptansprechpartner in der Firma");

            contact.Title = changeContact.Title;
            contact.Firstname = changeContact.Firstname;
            contact.Lastname = changeContact.Lastname;
            contact.Email = changeContact.Email;
            contact.TelNr = changeContact.TelNr;
            contact.MobilNr = changeContact.MobilNr;
            contact.Function = changeContact.Function;
            contact.MainPartner = changeContact.MainPartner;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { return BadRequest(e.InnerException?.Message ?? e.Message); }
            return Ok(contact.Guid);
        }

    }

}
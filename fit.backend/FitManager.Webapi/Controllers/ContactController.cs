namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ContactController : ControllerBase
    {
        private readonly PackageEventService _service;
        private readonly FitContext _db;

        public ContactController(FitContext db, PackageEventService service)
        {
            _db = db;
            _service = service;
        }

        public ChangeCmd(string id,string title, string firstname, string lastname, string email, string telNr, string? mobilNr, string function)
        [HttpPost]
        public async Task<IActionResult> ChangeContact([FromBody]ChangeCmd changeContact)
        {
            var contact = await _db.Contacts.FindAsync(changeContact.Id);
            if (contact == null)
                    return NotFound("Kontakt nicht gefunden");   
            
            contact.Title = changeContact.Title;
            contact.FirstName = changeContact.FirstName;
            contact.LastName = changeContact.LastName;
            contact.Email = changeContact.Email;
            contact.TelNr = changeContact.TelNr;
            contact.MobilNr = changeContact.MobilNr;
            contact.Function = changeContact.Function;

            await _db.SaveChangesAsync();
            return Ok("Kontakt erfolgreich geändert");
            }


    }

}
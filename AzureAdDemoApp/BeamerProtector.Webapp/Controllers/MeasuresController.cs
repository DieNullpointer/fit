using Microsoft.AspNetCore.Mvc;

namespace BeamerProtector.Webapp.Controllers
{
    /// <summary>
    /// Wird vom Überwachungsmodul aufgerufen.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        /// <summary>
        /// Reagiert auf POST /api/meatures
        /// Todo: Füge eine Command Klasse mit den Messwerten hinzu und verarbeite
        /// den Request.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddMeasurement()
        {
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllMeasurements()
        {
            return Ok();
        }
    }
}
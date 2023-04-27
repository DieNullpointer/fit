using FitManager.Application.Infrastructure;
using FitManager.Webapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly AzureAdClient _adClient;
        private readonly FitContext _db;

        public MailController(FitContext db, AzureAdClient adClient)
        {
            _db = db;
            _adClient = adClient;
        }
    }
}

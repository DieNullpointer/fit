using BeamerProtector.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BeamerProtector.Webapp.Pages.Config
{
    public class IndexModel : PageModel
    {
        private readonly BeamerProtectorContext _db;

        public IndexModel(BeamerProtectorContext db)
        {
            _db = db;
        }

        public Application.Model.Config Config { get; private set; } = default!;
        public bool HasMailaccount => !string.IsNullOrEmpty(Config.MailerAccountname);

        public async Task OnGet()
        {
            Config = await _db.GetConfig();
        }
    }
}
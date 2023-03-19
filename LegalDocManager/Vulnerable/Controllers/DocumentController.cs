using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace Vulnerable.Controllers
{
    public class DocumentController : BaseController
    {
        public DocumentController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

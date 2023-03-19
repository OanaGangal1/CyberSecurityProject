using Microsoft.AspNetCore.Mvc;
using VulnerableApp.DataLayer;

namespace VulnerableApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppDbContext context;

        public BaseController(AppDbContext context)
        {
            this.context = context;
        }
    }
}

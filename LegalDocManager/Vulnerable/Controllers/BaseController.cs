using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace Vulnerable.Controllers
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

using Dependencies.DataLayer;
using Dependencies.Entities.Improved;
using Microsoft.AspNetCore.Mvc;

namespace Improved.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ImprovedDbContext context;

        public BaseController(ImprovedDbContext context)
        {
            this.context = context;
        }

        protected User? ValidateUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            var user = context.Users.FirstOrDefault(x => x.Id.ToString() == userId);
            return user;
        }
    }
}

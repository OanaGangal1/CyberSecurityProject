using Dependencies.DataLayer;
using Dependencies.Entities.Improved;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Improved.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ImprovedDbContext context;
        protected readonly UserManager<User> userManager;

        public BaseController(ImprovedDbContext context , UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        protected async Task<User?> ValidateUserAsync()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }
    }
}

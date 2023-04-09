using Dependencies.DataLayer;
using Dependencies.Entities.Vulnerable;
using Microsoft.AspNetCore.Mvc;

namespace Vulnerable.Controllers
{
    public class BaseController : Controller
    {
        protected readonly VulnerableDbContext context;

        public BaseController(VulnerableDbContext context)
        {
            this.context = context;
        }

        protected User? ValidateUser()
        {
            var username = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var user = context.Users.FirstOrDefault(x => x.UserName == username);
            return user;
        }
    }
}

using Dependencies.DataLayer;
using Dependencies.DataLayer.Entities;
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

        protected User? ValidateUser()
        {
            var username = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var user = context.Users.FirstOrDefault(x => x.UserName == username);
            return user;
        }
    }
}

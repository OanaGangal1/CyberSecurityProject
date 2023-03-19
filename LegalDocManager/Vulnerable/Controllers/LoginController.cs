using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Vulnerable.Models;

namespace Vulnerable.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticationModel model)
        {
            var user = context.Users
                .FirstOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            if (user == null)
                return BadRequest("Wrong username or password!");

            return await Task.FromResult(Ok(Guid.NewGuid().ToString()));
        }
    }
}

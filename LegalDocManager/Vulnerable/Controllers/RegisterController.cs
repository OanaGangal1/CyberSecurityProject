using Dependencies.DataLayer;
using Dependencies.Entities.Vulnerable;
using Microsoft.AspNetCore.Mvc;
using Vulnerable.Models;

namespace Vulnerable.Controllers
{
    [Controller]
    public class RegisterController : BaseController
    {
        public RegisterController(VulnerableDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticationModel model)
        {
            var user = context.Users.FirstOrDefault(x => x.UserName == model.Username);

            if (user != null)
                return BadRequest("Username already used!");

            user = new User
            {
                UserName = model.Username,
                Password = model.Password
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok("Registration was successful!");
        }
    }
}

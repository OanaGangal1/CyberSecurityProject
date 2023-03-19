using Dependencies.DataLayer;
using Dependencies.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Vulnerable.Models;

namespace Vulnerable.Controllers
{
    [Controller]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(AppDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthenticationModel model)
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
            var wasAdded = (await context.SaveChangesAsync()) > 0;
            return Ok(wasAdded);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticationModel model)
        {
            var user = context.Users
                .FirstOrDefault(x => x.UserName == model.Username && x.Password == model.Password);

            if (user == null)
                return BadRequest("Wrong username or password!");

            return await Task.FromResult(Ok(Guid.NewGuid().ToString()));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VulnerableApp.DataLayer;
using VulnerableApp.DataLayer.Entities;
using VulnerableApp.Models;

namespace VulnerableApp.Controllers
{
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(AppDbContext context) : base(context)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthenticationModel model)
        {
            var user = new User
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
            return Ok(string.Empty);
        }
    }
}

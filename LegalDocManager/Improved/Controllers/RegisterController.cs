using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Improved.Models;
using Dependencies.Entities.Improved;
using Microsoft.AspNetCore.Identity;

namespace Improved.Controllers
{
    [Controller]
    public class RegisterController : BaseController
    {

        public RegisterController(ImprovedDbContext context, UserManager<User> userManager) : base(context, userManager)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticationModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByNameAsync(model.Username);

            if (user != null)
                return BadRequest("Username already used!");

            user = new User
            {
                UserName = model.Username
            };

            var wasCreated = await userManager.CreateAsync(user);
            
            if (wasCreated.Succeeded)
            {
                await userManager.AddPasswordAsync(user, model.Password);
                return Ok("Registration was successful!");
            }

            return BadRequest("User could not be created!");
        }
    }
}

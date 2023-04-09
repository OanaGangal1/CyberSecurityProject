using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Improved.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dependencies.Entities.Improved;
using Microsoft.AspNetCore.Identity;

namespace Improved.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;

        public LoginController(
            ImprovedDbContext context, 
            IConfiguration configuration,
            SignInManager<User> signInManager,
            UserManager<User> userManager) : base(context, userManager)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticationModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return BadRequest("Wrong username or password!");

            var result = await signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (!result.Succeeded)
                return BadRequest("Invalid credentials!");

            var token = CreateToken(user);

            return await Task.FromResult(Ok(token));
        }

        private string CreateToken(User user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddDays(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenHandler.WriteToken(token);

            return tokenHandler.WriteToken(token);
        }
    }
}

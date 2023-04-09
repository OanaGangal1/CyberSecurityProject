using Dependencies.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Improved.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dependencies.Entities.Improved;

namespace Improved.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IConfiguration configuration;

        public LoginController(ImprovedDbContext context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;
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
            var jwtToken = tokenHandler.WriteToken(token);

            return tokenHandler.WriteToken(token);
        }
    }
}

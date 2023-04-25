using System.ComponentModel.DataAnnotations;

namespace Improved.Models
{
    public class AuthenticationModel
    {
        [MinLength(8)]
        public string Username { get; set; }

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "The password should be at least 8 characters long and contain at least one number, one uppercase, one lowercase and a special character")]
        public string Password { get; set; }
    }
}

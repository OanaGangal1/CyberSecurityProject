using Microsoft.AspNetCore.Identity;

namespace Dependencies.Entities.Improved
{
    public class User : IdentityUser<Guid>
    {
        public List<Document> Documents { get; set; } = new();
    }
}

using Dependencies.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace Improved.Extensions
{
    public static class StartupExtensions
    {
        public static void DbMigrate(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ImprovedDbContext>();
            context.Database.Migrate();
        }
    }
}

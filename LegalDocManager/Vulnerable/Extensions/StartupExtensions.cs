using Dependencies.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace Vulnerable.Extensions
{
    public static class StartupExtensions
    {
        public static void DbMigrate(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<VulnerableDbContext>();
            context.Database.Migrate();
        }

        private static string GetDbPath()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return System.IO.Path.Join(path, "v-legaldoc.db");
        }
        public static string ConnectionString = $"Data Source={GetDbPath()}";
    }
}

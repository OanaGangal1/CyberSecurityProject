﻿using Dependencies.DataLayer;
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

        public static string ConnectionString = "Server=localhost; Database=v-legaldoc; Integrated Security=True;";
    }
}

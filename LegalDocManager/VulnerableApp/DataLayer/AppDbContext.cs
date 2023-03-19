﻿using Microsoft.EntityFrameworkCore;
using VulnerableApp.DataLayer.Entities;

namespace VulnerableApp.DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}

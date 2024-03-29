﻿using Dependencies.Entities.Vulnerable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Dependencies.DataLayer
{
    public class VulnerableDbContext : DbContext
    {
        private string _dbPath;
        public VulnerableDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            _dbPath = System.IO.Path.Join(path, "v-legaldoc.db");
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

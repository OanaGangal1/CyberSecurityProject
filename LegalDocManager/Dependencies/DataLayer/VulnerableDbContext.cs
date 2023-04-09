using Dependencies.Entities.Vulnerable;
using Microsoft.EntityFrameworkCore;

namespace Dependencies.DataLayer
{
    public class VulnerableDbContext : DbContext
    {
        public VulnerableDbContext(DbContextOptions<VulnerableDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

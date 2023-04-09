using Dependencies.Entities.Improved;
using Microsoft.EntityFrameworkCore;

namespace Dependencies.DataLayer
{
    public class ImprovedDbContext : DbContext
    {
        public ImprovedDbContext(DbContextOptions<ImprovedDbContext> options) : base(options)
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

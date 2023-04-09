using Dependencies.Entities.Improved;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dependencies.DataLayer
{
    public class ImprovedDbContext : IdentityDbContext<User, IdentityRole<Guid> ,Guid>
    {
        public ImprovedDbContext(DbContextOptions<ImprovedDbContext> options) : base(options)
        {

        }

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

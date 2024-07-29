using Microsoft.EntityFrameworkCore;
using UserList.API.Models;

namespace UserList.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entity = modelBuilder.Entity<User>();

            entity.ToTable("users");

            entity.Property(u => u.Id)
                  .HasColumnName("id");

            entity.Property(u => u.Name)
                  .HasColumnName("name");

            entity.Property(u => u.Surname)
                  .HasColumnName("surname");

            entity.Property(u => u.Email)
                  .HasColumnName("email")
                  .IsRequired(false);
        }
    }
}

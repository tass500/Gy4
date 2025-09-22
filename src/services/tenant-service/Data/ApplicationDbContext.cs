using Microsoft.EntityFrameworkCore;
using TenantService.Models;

namespace TenantService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Tenant entity
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.ConnectionString)
                    .HasMaxLength(255);
                    
                entity.Property(e => e.IsActive);
                    
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                
                // Add indexes
                entity.HasIndex(e => e.Domain).IsUnique();
                entity.HasIndex(e => e.Name);
                
                // Seed data - simplified to avoid duplicates
                entity.HasData(
                    new Tenant
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Teszt Bérlő 1",
                        Domain = "teszt1.local",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tenant
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Teszt Bérlő 2",
                        Domain = "teszt2.local",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            });
        }
    }
}

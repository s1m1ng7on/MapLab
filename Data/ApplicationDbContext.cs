using MapLab.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapTemplate> MapTemplates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Map>()
                .HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Map>()
                .HasOne(m => m.Template)
                .WithMany()
                .HasForeignKey(m => m.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

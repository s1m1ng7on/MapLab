using MapLab.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Data
{
    public class ApplicationDbContext : IdentityDbContext<Profile>
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
                .HasOne(m => m.Profile)
                .WithMany()
                .HasForeignKey(m => m.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Map>()
                .HasOne(m => m.Template)
                .WithMany()
                .HasForeignKey(m => m.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

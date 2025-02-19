using MapLab.Data.Entities;
using MapLab.Data.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MapLab.Data
{
    public class ApplicationDbContext : IdentityDbContext<Profile>
    {
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapTemplate> MapTemplates { get; set; }
        public DbSet<ProfileRecentMapTemplate> ProfileRecentMapTemplates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MapTemplate>()
                .Property(mt => mt.Region)
                .HasConversion<int>();

            builder.Entity<Map>()
                .HasOne(m => m.Profile)
                .WithMany(p => p.Maps)
                .HasForeignKey(m => m.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MapTemplate>()
                .HasOne(m => m.Profile)
                .WithMany(p => p.MapTemplates)
                .HasForeignKey(m => m.ProfileId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Map>()
                .HasOne(m => m.Template)
                .WithMany(m => m.Maps)
                .HasForeignKey(m => m.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

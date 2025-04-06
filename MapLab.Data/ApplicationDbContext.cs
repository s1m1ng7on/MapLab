using MapLab.Data.Configurations;
using MapLab.Data.Entities;
using MapLab.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace MapLab.Data
{
    public class ApplicationDbContext : IdentityDbContext<Profile>
    {
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapTemplate> MapTemplates { get; set; }
        public DbSet<MapView> MapViews { get; set; }
        public DbSet<NewsArticle> News { get; set; }
        public DbSet<Like<Map>> MapLikes { get; set; }
        public DbSet<Like<MapTemplate>> MapTemplateLikes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            GlobalFilters.ConfigureDeletableEntityFilters(builder);

            builder.Entity<Profile>().ToTable("Profiles");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("ProfileRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("ProfileClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("ProfileLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("ProfileTokens");

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

            builder.Entity<Like<Map>>(ml =>
            {
                ml.Property(ml => ml.EntityId)
                    .HasColumnName("MapId");

                ml.HasOne(e => e.Profile)
                    .WithMany()
                    .HasForeignKey(e => e.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Like<MapTemplate>>(mtl =>
            {
                mtl.Property(mtl => mtl.EntityId)
                    .HasColumnName("MapTemplateId");

                mtl.HasOne(e => e.Profile)
                    .WithMany()
                    .HasForeignKey(e => e.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

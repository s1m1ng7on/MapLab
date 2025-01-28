using MapLab.Data;
using MapLab.Data.Entities;
using MapLab.Data.Repositories;
using MapLab.Data.Seeding;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Templates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MapLab
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigureServices();
            var app = builder.Build();
            app.Configure();
            app.Run();
        }

        private static void ConfigureServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Lockout.MaxFailedAccessAttempts = 10;
                }
                else
                {
                    options.SignIn.RequireConfirmedAccount = true;
                }
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddSingleton(builder.Configuration);

            // Enforce lowercase routes
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            // Modify Identity route to use "/Auth" instead of "/Identity/Account"
            builder.Services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    //SETUP IDENTITY CUSTOM ROUTES
                });

            // Data repositories
            builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(DeletableEntityRepository<>));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // TODO: Move configuration and make different configurations
            // Register AutoMapper
            builder.Services.AddAutoMapper(c =>
            {
                c.CreateMap<UploadViewModel, MapTemplate>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File));
            });

            // Application services
            builder.Services.AddTransient<IFileStorageService, LocalFileStorageService>();
            builder.Services.AddTransient<IMapService, MapService>();
        }

        private static void Configure(this WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(context, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.MapBlazorHub();
        }
    }
}

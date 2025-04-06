using Cropper.Blazor.Extensions;
using MapLab.Data;
using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Data.Seeding;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

            builder.Services.AddScoped<ProfileManager<Profile>>();

            builder.Services.AddDefaultIdentity<Profile>(options =>
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

            builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddRazorRuntimeCompilation();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("bg"),
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddCropper();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSingleton(builder.Configuration);
            //builder.Services.AddSingleton<TempDataSerializer>();

            // Enforce lowercase routes
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            // Modify Identity route to use "/Auth" instead of "/Identity/Account"
            builder.Services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    /*options.Conventions.AuthorizeAreaFolder("Identity", "/Account");
                    options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account", model =>
                    {
                        foreach (var selector in model.Selectors)
                        {
                            selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template
                                .Replace("Identity/Account/", string.Empty);
                        }
                    });*/
                });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
            });


            // Data repositories
            builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(DeletableEntityRepository<>));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddTransient<IViewRenderService, ViewRenderService>();

            // File storage manager
            builder.Services.AddTransient<IFileStorageManager, LocalFileStorageManager>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddTransient<INotifierService, NotifierService>();

            builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration).Assembly);

            // You can also explicitly register your AutoMapper configuration class like so:
            builder.Services.AddSingleton(provider =>
            {
                AutoMapperConfiguration.RegisterMappings(provider, AppDomain.CurrentDomain.GetAssemblies());
                return AutoMapperConfiguration.MapperInstance;
            });

            // Application services
            builder.Services.AddTransient<IProfileService, ProfileService>();
            builder.Services.AddTransient<IMapsService, MapsService>();
            builder.Services.AddTransient<ITemplatesService, TemplatesService>();
            builder.Services.AddTransient<IMapAnalyticsService, MapAnalyticsService>();
            builder.Services.AddTransient<INewsService, NewsService>();
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
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/notfound");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseRequestLocalization();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAreaControllerRoute(
                name: "admin",
                areaName: "Admin",
                pattern: "Admin/{controller=Dashboard}/{action=Index}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.MapRazorPages();
            app.MapBlazorHub();
        }
    }
}

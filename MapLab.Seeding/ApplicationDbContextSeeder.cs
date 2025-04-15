using MapLab.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MapLab.Seeding
{
    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<ApplicationDbContextSeeder>();

            var seeders = new List<ISeeder>()
            {
                new RolesSeeder(),
                new ProfileSeeder(),
                new SystemAccountSeeder(),
                new MapTemplateSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context, serviceProvider);
                await context.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}

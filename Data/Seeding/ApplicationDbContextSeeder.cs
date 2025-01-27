using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLab.Data.Seeding
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
                new RolesSeeder()
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

using MapLab.Data;
using MapLab.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapLab.Seeding
{
    internal class MapSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            /*var mapsService = serviceProvider.GetService<IMapsService>();

            await mapsService.CreateMapAsync()*/
        }
    }
}

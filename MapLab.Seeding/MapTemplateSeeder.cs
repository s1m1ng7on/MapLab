using MapLab.Data;
using MapLab.Seeding.Data;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Seeding
{
    internal class MapTemplateSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var mapTemplateService = serviceProvider.GetService<ITemplatesService>();

            List<(MapTemplateDto, string)> mapTemplates = MapTemplates.InitialMapTemplates;

            if (context.MapTemplates.Any()) return;

            var userIds = await context.Users
                .Where(u => u.Id != null)
                .Select(u => u.Id)
                .ToListAsync();

            if (userIds.Count == 0)
                throw new InvalidOperationException("No users found in the system.");

            foreach (var (dto, filePath) in mapTemplates)
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Files", "MapTemplates", filePath);

                if (!File.Exists(fullPath)) throw new FileNotFoundException($"The file '{fullPath}' was not found.");

                using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                var mapTemplateFile = new FormFile(stream, 0, stream.Length, "File", filePath);

                var randomUserId = userIds[new Random().Next(userIds.Count)];

                dto.ProfileId = randomUserId;

                await mapTemplateService.UploadMapTemplateAsync(dto, mapTemplateFile);
            }
        }
    }
}

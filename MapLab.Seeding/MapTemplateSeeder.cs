using MapLab.Data;
using MapLab.Seeding.Data;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MapLab.Seeding
{
    internal class MapTemplateSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var mapTemplateService = serviceProvider.GetService<ITemplatesService>();

            List<(MapTemplateDto, string)> mapTemplates = MapTemplates.InitialMapTemplates;

            if (context.MapTemplates.Any()) return;

            foreach (var (dto, filePath) in mapTemplates)
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Files", "MapTemplates", filePath);

                if (!File.Exists(fullPath)) throw new FileNotFoundException($"The file '{fullPath}' was not found.");

                // Create the FormFile for the upload
                using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                var mapTemplateFile = new FormFile(stream, 0, stream.Length, "File", filePath);

                dto.ProfileId = "19ec88a2-845d-4f50-89b0-cf691652fb42";

                await mapTemplateService.UploadMapTemplateAsync(dto, mapTemplateFile);
            }
        }
    }
}

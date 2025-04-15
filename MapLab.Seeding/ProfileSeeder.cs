using MapLab.Data;
using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Data.Managers.Contracts;
using MapLab.Seeding.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MapLab.Seeding
{
    internal class ProfileSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (context.Users.Any()) return;

            var profileManager = serviceProvider.GetService<ProfileManager<Profile>>();
            var fileStorageManager = serviceProvider.GetService<IFileStorageManager>();

            var profiles = Profiles.InitialProfiles;

            foreach (var (profile, profilePictureFilePath) in profiles)
            {
                profile.EmailConfirmed = true;

                if (!string.IsNullOrEmpty(profilePictureFilePath))
                {
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Files", "Profile", "ProfilePictures", profilePictureFilePath);

                    if (!File.Exists(fullPath)) throw new FileNotFoundException($"The file '{fullPath}' was not found.");

                    using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                    var mapTemplateFile = new FormFile(stream, 0, stream.Length, "File", fullPath);
                    profile.ProfilePictureFilePath = await fileStorageManager!.SaveFileAsync(mapTemplateFile, "Profiles", "ProfilePicture", profile.Id);
                }

                var result = await profileManager!.CreateAsync(profile, "DefaultPassword123!");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

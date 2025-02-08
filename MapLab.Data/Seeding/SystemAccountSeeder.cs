using MapLab.Data.Entities;
using MapLab.Data.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MapLab.Data.Seeding
{
    internal class SystemAccountSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var profileManager = serviceProvider.GetRequiredService<ProfileManager<Profile>>();
            var configurationService = serviceProvider.GetRequiredService<IConfiguration>();

            var userName = configurationService["SystemAccount:UserName"];

            var profile = await profileManager.FindByNameAsync(userName);

            if (profile == null)
            {

                var email = configurationService["SystemAccount:Email"];
                var password = configurationService["SystemAccount:Password"];

                profile = new Profile
                {
                    UserName = userName,
                    Email = email,
                    CreatedOn = new DateTime(2024, 12, 1)
                };

                var createProfileResult = await profileManager.CreateAsync(profile, password);

                if (!createProfileResult.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, createProfileResult.Errors.Select(e => e.Description)));
                }
            }

            await AddToRoleAsync(profileManager, profile, "Admin");
            await AddToRoleAsync(profileManager, profile, "System");
        }

        public static async Task AddToRoleAsync(ProfileManager<Profile> profileManager, Profile profile, string roleName)
        {
            var isInRole = await profileManager.IsInRoleAsync(profile, roleName);

            if (!isInRole)
            {
                var addToRoleResult = await profileManager.AddToRoleAsync(profile, roleName);

                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, addToRoleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

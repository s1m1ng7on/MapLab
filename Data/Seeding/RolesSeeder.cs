using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MapLab.Data.Seeding
{
    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var configurationService = serviceProvider.GetRequiredService<IConfiguration>();

            var roleNames = configurationService.GetSection("Roles").Get<string[]>();

            foreach (var roleName in roleNames)
            {
                await SeedRoleAsync(roleManager, roleName);
            }
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

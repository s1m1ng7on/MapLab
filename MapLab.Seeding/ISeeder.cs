using MapLab.Data;

namespace MapLab.Seeding
{
    public interface ISeeder
    {
        public Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider);
    }
}

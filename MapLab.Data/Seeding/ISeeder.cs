namespace MapLab.Data.Seeding
{
    public interface ISeeder
    {
        public Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider);
    }
}

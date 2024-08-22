namespace SpiceCraft.Server.SeedData
{
    public class Seed
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            await UsersSeed.SeedUsers(serviceProvider);
        }
    }
}

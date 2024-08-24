using SpiceCraft.Server.Repository.Interface;
using SpiceCraft.Server.Repository;

namespace SpiceCraft.Server
{
    public static class DIContainer
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register IUserRepository with UserRepository implementation
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}

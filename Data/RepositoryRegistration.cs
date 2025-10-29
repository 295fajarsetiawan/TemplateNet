using Data.Repository;
using Data.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class RepositoryRegistration
{
    public static void RegisterRepositoryService(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
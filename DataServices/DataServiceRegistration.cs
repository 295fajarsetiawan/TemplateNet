using DataServices.Service;
using DataServices.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices;

public static class DataServiceRegistration
{
    public static void RegisterDataService(this IServiceCollection services)
    {
        services.AddTransient<IUserDataService, UserDataService>();
    }
}

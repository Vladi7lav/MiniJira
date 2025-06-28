using Microsoft.Extensions.DependencyInjection;
using MiniJira.DomainServices.Services.Tasks;
using MiniJira.DomainServices.Services.Users;

namespace MiniJira.DomainServices;

public static class DomainServicesRegistrar
{
    public static void Configure(IServiceCollection services)
    {
        services.AddSingleton<ITaskService, TaskService>();
        services.AddSingleton<IUserService, UserService>();
    }
}
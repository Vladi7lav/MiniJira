using Microsoft.Extensions.DependencyInjection;
using MiniJira.Infrastructure.Logs;
using MiniJira.Infrastructure.Tasks;
using MiniJira.Infrastructure.Users;
using MiniJira.Repository;

namespace MiniJira.Infrastructure;

public static class InfrastructureRegistrar
{
    public static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<ITaskRepository, TaskRepository>();
        services.AddSingleton<ILogRepository, LogRepository>();
    }
}


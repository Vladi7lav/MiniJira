using MiniJira.DomainServices;
using MiniJira.Infrastructure;
using MiniJira.Presentation.Interfaces;
using MiniJira.Presentation.UserInterfaceServices;

namespace MiniJira.Presentation;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                InfrastructureRegistrar.Configure(services);
                DomainServicesRegistrar.Configure(services);
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IUserInterfaceActionsService, UserInterfaceActionsService>();
                services.AddSingleton<ITaskInterfaceActionsService, TaskInterfaceActionsService>();
                services.AddHostedService<MainInterfaceWorker>();
            });
        var app = builder.Build();
        await app.RunAsync();
    }
}

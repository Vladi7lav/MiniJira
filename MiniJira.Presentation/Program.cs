using MiniJira.Infrastructure;
using MiniJira.Presentation.Workers;

namespace MiniJira.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                InfrastructureRegistrar.Configure(services);
                services.AddHostedService<MainInterfaceWorker>();
            });
        var app = builder.Build();
        await app.RunAsync();
    }
}

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniJira.Presentation.Workers;

namespace MiniJira.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MainInterfaceWorker>();
                });
            var app = builder.Build();
            await app.RunAsync();
        }
    }
}
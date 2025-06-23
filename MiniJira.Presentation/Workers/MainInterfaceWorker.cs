using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MiniJira.Presentation.Workers
{
    public class MainInterfaceWorker : IHostedService
    {
        public MainInterfaceWorker()
        {
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
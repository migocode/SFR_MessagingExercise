using Microsoft.Extensions.Hosting;
using MoneyLaunderingService.Interfaces;

namespace MoneyLaunderingService
{
    public class MoneyLaundryHostedService : IHostedService, IDisposable
    {
        private Task? consumerTask;
        private readonly IMessageConsumer consumer;

        public MoneyLaundryHostedService(IMessageConsumer consumer)
        {
            this.consumer = consumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            consumerTask = Task.Run(() => consumer.StartConsumption(cancellationToken), cancellationToken);
            return consumerTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            consumerTask?.Dispose();
        }
    }
}

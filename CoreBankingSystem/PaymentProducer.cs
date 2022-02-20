using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Messages;
using System.Net;
using System.Text.Json;

namespace CoreBankingSystem
{
    internal class PaymentProducer : IHostedService, IDisposable
    {
        private Timer? timer = null;
        private IProducer<Null, string>? producer;
        private readonly Settings settings;

        public PaymentProducer(IOptions<Settings> options)
        {
            settings = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(PaymentProducer)} started.");

            ProducerConfig producerConfig = new ProducerConfig
            {
                BootstrapServers = settings.KafkaServers,
                ClientId = Dns.GetHostName(),
                EnableDeliveryReports = true,
            };

            producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            timer = new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(2000));

            return Task.CompletedTask;
        }

        private void SendMessage(object? state)
        {
            Random random = new Random();
            random.Next();
            Payment payment = new(random.Next(0, 2_000), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            string serializedPayment = JsonSerializer.Serialize(payment);

            Console.WriteLine($"Sending message to kafka-topic {settings.TopicPayments}");
            producer?.Produce(settings.TopicPayments, new() { Value = serializedPayment }, HandleDeliveryReport);
        }

        private void HandleDeliveryReport(DeliveryReport<Null, string> deliveryReport)
        {
            Console.WriteLine($"Message-Status: {deliveryReport.Status}, Error: {deliveryReport.Error.Reason}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            producer?.Flush(cancellationToken);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
            producer?.Dispose();
        }
    }
}

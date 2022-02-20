using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MoneyLaunderingService.Interfaces;
using Shared.Messages;
using System.Net;
using System.Text.Json;

namespace MoneyLaunderingService
{
    public class MessageProducer : IMessageProducer, IDisposable
    {
        private IProducer<Null, string>? producer;
        private readonly Settings settings;

        public MessageProducer(IOptions<Settings> options)
        {
            settings = options.Value;

            ProducerConfig producerConfig = new()
            {
                BootstrapServers = settings.KafkaServers,
                ClientId = Dns.GetHostName(),
                EnableDeliveryReports = true,
            };

            producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public void SendMessage(LaundryCheckResult laundryCheck)
        {
            string serializedLaundryCheck = JsonSerializer.Serialize(laundryCheck);

            Console.WriteLine($"Sending message to kafka-topic {settings.TopicLaundryCheck}");
            producer?.Produce(settings.TopicLaundryCheck, new() { Value = serializedLaundryCheck }, HandleDeliveryReport);
        }

        private void HandleDeliveryReport(DeliveryReport<Null, string> deliveryReport)
        {
            Console.WriteLine($"Message-Status: {deliveryReport.Status}, Error: {deliveryReport.Error.Reason}");
        }

        public void Dispose()
        {
            producer?.Flush();
            producer?.Dispose();
        }
    }
}

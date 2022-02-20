using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MoneyLaunderingService.Interfaces;
using Shared.Messages;
using System.Net;
using System.Text.Json;

namespace MoneyLaunderingService
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly ConsumerConfig config;
        private readonly Settings settings;
        private readonly IMoneyLaundryChecker checker;
        private readonly IMessageProducer producer;

        public MessageConsumer(IOptions<Settings> options,
            IMoneyLaundryChecker checker,
            IMessageProducer producer)
        {
            settings = options.Value;
            this.checker = checker;
            this.producer = producer;

            config = new ConsumerConfig
            {
                BootstrapServers = settings.KafkaServers,
                GroupId = Dns.GetHostName(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public void StartConsumption(CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(settings.TopicPayments);

                while (!cancellationToken.IsCancellationRequested)
                {
                    ConsumeResult<Ignore, string>? consumeResult = null;

                    try
                    {
                        consumeResult = consumer.Consume(cancellationToken);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    if (consumeResult?.Message?.Value is null)
                    {
                        continue;
                    }

                    Console.WriteLine(consumeResult.Message.Value);
                    Payment? payment = JsonSerializer.Deserialize<Payment>(consumeResult.Message.Value);

                    if(payment is null)
                    {
                        continue;
                    }

                    LaundryCheckResult checkResult = checker.Check(payment);
                    producer.SendMessage(checkResult);
                }

                consumer.Close();
            }
        }
    }
}

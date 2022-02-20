using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Messages;
using System.Net;
using System.Text.Json;
using TransactionAnalysisService.Interfaces;

namespace TransactionAnalysisService
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly ConsumerConfig config;
        public readonly Settings settings;
        private readonly ITransactionAnalysisProcessor analysProcessor;

        public MessageConsumer(IOptions<Settings> options,
            ITransactionAnalysisProcessor analysProcessor)
        {
            settings = options.Value;
            this.analysProcessor = analysProcessor;

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
                consumer.Subscribe(settings.TopicLaundryCheck);

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
                    LaundryCheckResult? result = JsonSerializer.Deserialize<LaundryCheckResult>(consumeResult.Message.Value);

                    if (result is null)
                    {
                        continue;
                    }

                    analysProcessor.ProcessTransaction(result);
                }

                consumer.Close();
            }
        }
    }
}

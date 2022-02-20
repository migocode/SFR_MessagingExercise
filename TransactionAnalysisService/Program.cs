using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionAnalysisService;
using TransactionAnalysisService.Interfaces;

Console.WriteLine($"Starting {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name} (Producer)");

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<TransactionAnalysisHostedService>();
        services.Configure<Settings>(hostContext.Configuration);

        services.AddTransient<ITransactionAnalysisProcessor, TransactionAnalysisProcessor>();
        services.AddTransient<IMessageConsumer, MessageConsumer>();
    })
    .Build()
    .Run();
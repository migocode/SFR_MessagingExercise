using CoreBankingSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine($"Starting {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name} (Producer)");

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<PaymentProducer>();
        services.Configure<Settings>(hostContext.Configuration);
    })
    .Build()
    .Run();
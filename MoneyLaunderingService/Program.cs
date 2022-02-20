using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyLaunderingService;
using MoneyLaunderingService.Interfaces;

Console.WriteLine($"Starting {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name} (Producer)");

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<MoneyLaundryHostedService>();
        services.Configure<Settings>(hostContext.Configuration);

        services.AddTransient<IMessageConsumer, MessageConsumer>();
        services.AddTransient<IMoneyLaundryChecker, MoneyLaundryChecker>();
        services.AddTransient<IMessageProducer, MessageProducer>();
    })
    .Build()
    .Run();
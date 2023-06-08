using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService(options =>
        {
            options.ServiceName = ".NET Joke Service";
        })
        .ConfigureLogging((hostBuilderContext, logging) =>
        {
            logging.AddConsole();
            logging.AddFileLogger(options =>
            {
                hostBuilderContext.Configuration.GetSection("Logging").GetSection("FileLogger").GetSection("Options").Bind(options);
            });
        })
    .ConfigureServices((context, services) =>
    {       
        services.AddSingleton<JokeService>();
        services.AddHostedService<WindowsBackgroundService>();

        services.AddLogging(builder =>
        {
            builder.AddConfiguration(
                context.Configuration.GetSection("Logging"));
        });
    })
    .Build();

await host.RunAsync();

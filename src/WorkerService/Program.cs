using Serilog;
using Serilog.Events;
using WorkerService.Commands.Implementations;
using WorkerService.Commands.Interfaces;
using WorkerService.Options;
using WorkerService.Repositories.Implementations;
using WorkerService.Repositories.Interfaces;
using WorkerService.Services.Implementations;
using WorkerService.Services.Interfaces;
using WorkerService.Workers;

// https://github.com/serilog/serilog-aspnetcore
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log-.txt"),
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 10 * 1024 * 1024,
        retainedFileCountLimit: 2,
        rollOnFileSizeLimit: true,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1),
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

var builder = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureAppConfiguration(ConfigureAppConfiguration)
    .ConfigureServices(ConfigureServices);

var host = builder.Build();

try
{
    host.Run();
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Unhandled exception occurred in the application.");
}

return;

void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder configBuilder)
{
    configBuilder
        .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables("BlockchainObservatory__");
}

void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    services.AddHttpClient();
    services.AddSerilog();

    services.AddOptions<PriceAlertOptions>()
        .Bind(hostContext.Configuration.GetSection(PriceAlertOptions.ConfigSection))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    services.AddScoped<IBotCommand, PingBotCommand>();
    services.AddScoped<IBotCommand, StartBotCommand>();
    services.AddSingleton<IBotCommandHandler, BotCommandHandler>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IPriceChangeRepository, PriceChangeRepository>();
    services.AddScoped<ICryptoPriceService, CryptoPriceService>();
    services.AddHostedService<PriceAlertWorker>();

    services.AddSingleton<TelegramBotWorker>();
    services.AddSingleton<ITelegramAlertService>(provider =>
    {
        var telegramBotWorker = provider.GetService<TelegramBotWorker>();
        return telegramBotWorker ?? throw new InvalidOperationException($"{nameof(TelegramBotWorker)} not registered.");
    });
    services.AddHostedService<TelegramBotWorker>();
}
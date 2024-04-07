using WorkerService.Options;
using WorkerService.Services.Implementations;
using WorkerService.Services.Interfaces;
using WorkerService.Workers;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(ConfigureAppConfiguration)
    .ConfigureServices(ConfigureServices);

var host = builder.Build();
host.Run();
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
    // ️ Register framework services
    services.AddHttpClient();

    // ⚙️ Configure options from app configuration
    services.AddOptions<PriceAlertOptions>()
        .Bind(hostContext.Configuration.GetSection(PriceAlertOptions.ConfigSection))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // Register application-specific services
    services.AddScoped<ICryptoPriceService, CryptoPriceService>();
    services.AddHostedService<PriceAlertWorker>();
}
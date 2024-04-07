using WorkerService.Options;
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
    services.AddOptions<PriceAlertOptions>()
        .Bind(hostContext.Configuration.GetSection(PriceAlertOptions.ConfigSection))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    services.AddHostedService<PriceAlertWorker>();
}
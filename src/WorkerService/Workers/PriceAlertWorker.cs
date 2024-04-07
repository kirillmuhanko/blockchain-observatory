using Microsoft.Extensions.Options;
using WorkerService.Options;

namespace WorkerService.Workers;

public class PriceAlertWorker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var priceAlertOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<PriceAlertOptions>>();
            await Task.Delay(priceAlertOptions.Value.AutoRetryDelay, stoppingToken);
        }
    }
}
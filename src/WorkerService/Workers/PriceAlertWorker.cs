using Microsoft.Extensions.Options;
using WorkerService.Options;
using WorkerService.Services.Interfaces;

namespace WorkerService.Workers;

public class PriceAlertWorker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var priceAlertOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<PriceAlertOptions>>();
            var cryptoPriceService = scope.ServiceProvider.GetRequiredService<ICryptoPriceService>();
            // ReSharper disable once UnusedVariable
            var priceMovements = await cryptoPriceService.FetchLatestPriceMovementsAsync();
            await Task.Delay(priceAlertOptions.Value.AutoRetryDelay, stoppingToken);
        }
    }
}
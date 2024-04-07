using Microsoft.Extensions.Options;
using WorkerService.Options;
using WorkerService.Repositories.Interfaces;
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
            var priceChangeRepository = scope.ServiceProvider.GetRequiredService<IPriceChangeRepository>();
            var priceMovements = await cryptoPriceService.FetchLatestPriceMovementsAsync();
            // ReSharper disable once UnusedVariable
            var alerts = priceChangeRepository.GetPriceChangeAlerts(priceMovements);
            await Task.Delay(priceAlertOptions.Value.AutoRetryDelay, stoppingToken);
        }
    }
}
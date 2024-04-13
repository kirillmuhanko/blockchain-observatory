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
            var cryptoPriceService = scope.ServiceProvider.GetRequiredService<ICryptoPriceService>();
            var priceMovements = await cryptoPriceService.FetchLatestPriceMovementsAsync();

            var priceChangeRepository = scope.ServiceProvider.GetRequiredService<IPriceChangeRepository>();
            var alerts = priceChangeRepository.GetPriceChangeAlerts(priceMovements).ToList();

            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var users = await userRepository.GetUsersAsync();

            var telegramAlertService = scope.ServiceProvider.GetRequiredService<ITelegramAlertService>();
            await telegramAlertService.SendMessagesAsync(users, alerts);

            var priceAlertOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<PriceAlertOptions>>();
            await Task.Delay(priceAlertOptions.Value.AutoRetryDelay, stoppingToken);
        }
    }
}
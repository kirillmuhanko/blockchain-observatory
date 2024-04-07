using Microsoft.Extensions.Options;
using WorkerService.Models;
using WorkerService.Options;
using WorkerService.Repositories.Interfaces;

namespace WorkerService.Repositories.Implementations;

public class PriceChangeRepository(IOptionsSnapshot<PriceAlertOptions> priceAlertOptions) : IPriceChangeRepository
{
    private static readonly Dictionary<string, float> PriceChangeHistory = new();

    public IEnumerable<string> GetPriceChangeAlerts(IEnumerable<PriceMovementModel> priceMovements)
    {
        foreach (var movement in priceMovements)
        {
            var previousChangePercent = GetLatestPriceChange(movement.Symbol);
            var currentChangePercent = movement.PriceChangePercent;
            var priceChangeDifference = currentChangePercent - previousChangePercent;

            if (Math.Abs(priceChangeDifference) < priceAlertOptions.Value.MinChange)
                continue;

            UpdatePriceChangeHistory(movement.Symbol, currentChangePercent);

            if (currentChangePercent >= priceAlertOptions.Value.AlertThreshold)
                yield return $"{movement.Symbol} has increased by {movement.PriceChangePercent}%";
            else if (currentChangePercent <= -priceAlertOptions.Value.AlertThreshold)
                yield return $"{movement.Symbol} has decreased by {movement.PriceChangePercent}%";
        }
    }

    private static float GetLatestPriceChange(string cryptoSymbol)
    {
        PriceChangeHistory.TryGetValue(cryptoSymbol, out var lastPriceChangePercent);
        return lastPriceChangePercent;
    }

    private static void UpdatePriceChangeHistory(string cryptoSymbol, float priceChangePercent)
    {
        PriceChangeHistory[cryptoSymbol] = priceChangePercent;
    }
}
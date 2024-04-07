using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WorkerService.Models;
using WorkerService.Options;
using WorkerService.Services.Interfaces;

namespace WorkerService.Services.Implementations;

public class CryptoPriceService(
    IHttpClientFactory httpClientFactory,
    ILogger<CryptoPriceService> logger,
    IOptionsSnapshot<PriceAlertOptions> priceAlertOptions) : ICryptoPriceService
{
    public async Task<IEnumerable<PriceMovementModel>> FetchLatestPriceMovementsAsync()
    {
        var httpClient = httpClientFactory.CreateClient();

        var priceFetchTasks = priceAlertOptions.Value.Binance.Symbols.Select(async symbol =>
        {
            var priceData = await FetchPriceDataAsync(symbol, httpClient);
            return priceData;
        });

        var allFetchedPriceData = await Task.WhenAll(priceFetchTasks);

        var orderedPriceMovements = allFetchedPriceData
            .Where(data => data != null)
            .Select(data => data!)
            .OrderByDescending(t => t.PriceChangePercent)
            .AsEnumerable();

        return orderedPriceMovements;
    }

    private async Task<PriceMovementModel?> FetchPriceDataAsync(string symbol, HttpClient httpClient)
    {
        var url = $"{priceAlertOptions.Value.Binance.ApiUrl}{symbol}";

        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PriceMovementModel>(json);
            }

            logger.LogError($"Error fetching price data for {symbol}: {response.ReasonPhrase}");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError($"An unexpected error occurred fetching price data for {symbol}: {ex.Message}");
            return null;
        }
    }
}
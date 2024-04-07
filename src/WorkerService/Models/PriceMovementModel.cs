using Newtonsoft.Json;

namespace WorkerService.Models;

public class PriceMovementModel
{
    [JsonProperty("symbol")] public required string Symbol { get; set; }

    [JsonProperty("priceChangePercent")] public required float PriceChangePercent { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace WorkerService.Options;

// ReSharper disable once ClassNeverInstantiated.Global
public class BinanceOptions
{
    [Required] [Url] public required string ApiUrl { get; set; }

    [Required] [MinLength(1)] public required IEnumerable<string> Symbols { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace WorkerService.Options;

public class PriceAlertOptions : IValidatableObject
{
    public const string ConfigSection = "PriceAlertWorker";

    [Range(1f, 100f)] public required float AlertThreshold { get; set; }

    [Range(1f, 100f)] public required float MinChange { get; set; }

    [Required] public required TimeSpan AutoRetryDelay { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [Required] public required BinanceOptions Binance { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [Required] public required TelegramOptions Telegram { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validations = new List<ValidationResult>();

        if (AutoRetryDelay < TimeSpan.FromMinutes(1))
            validations.Add(new ValidationResult(
                $"{nameof(AutoRetryDelay)} must be greater than 1 minute.",
                new[] { nameof(AutoRetryDelay) }));

        Validator.TryValidateObject(Binance, new ValidationContext(Binance), validations, true);
        Validator.TryValidateObject(Telegram, new ValidationContext(Telegram), validations, true);

        return validations;
    }
}
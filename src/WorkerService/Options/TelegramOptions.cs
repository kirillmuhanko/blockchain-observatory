using System.ComponentModel.DataAnnotations;

namespace WorkerService.Options;

// ReSharper disable once ClassNeverInstantiated.Global
public class TelegramOptions
{
    [Required] public required string BotToken { get; set; }
}
using Microsoft.Extensions.Options;
using Telegram.Bot;
using WorkerService.Commands.Interfaces;
using WorkerService.Models;
using WorkerService.Options;
using WorkerService.Services.Interfaces;

namespace WorkerService.Workers;

public class TelegramBotWorker(
    IBotCommandHandler botCommandHandler,
    ILogger<TelegramBotWorker> logger,
    IOptions<PriceAlertOptions> options) : IHostedService, ITelegramAlertService
{
    private readonly ITelegramBotClient _telegramBotClient = new TelegramBotClient(options.Value.Telegram.BotToken);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _telegramBotClient.StartReceiving(
            (bot, update, _) => update.Message != null
                ? botCommandHandler.HandleCommandAsync(bot, update.Message)
                : Task.CompletedTask,
            (_, _, _) => Task.CompletedTask,
            cancellationToken: cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task SendMessagesAsync(IEnumerable<UserJsonModel> users, IEnumerable<string> messages)
    {
        var concatenatedMessages = string.Join(Environment.NewLine, messages);

        if (string.IsNullOrEmpty(concatenatedMessages))
            return Task.CompletedTask;

        var sendTasks = users.Select(user =>
        {
            try
            {
                return _telegramBotClient.SendTextMessageAsync(
                    user.ChatId,
                    concatenatedMessages,
                    cancellationToken: default);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error sending message to user {user.ChatId}: {ex.Message}");
                return Task.CompletedTask;
            }
        });

        return Task.WhenAll(sendTasks.ToList());
    }
}
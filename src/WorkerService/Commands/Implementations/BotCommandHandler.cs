using Telegram.Bot;
using Telegram.Bot.Types;
using WorkerService.Commands.Interfaces;

namespace WorkerService.Commands.Implementations;

public class BotCommandHandler(IServiceScopeFactory serviceScopeFactory) : IBotCommandHandler
{
    public async Task HandleCommandAsync(ITelegramBotClient client, Message message)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var commands = scope.ServiceProvider.GetRequiredService<IEnumerable<IBotCommand>>();

        if (message.Text == null)
            return;

        foreach (var command in commands)
            if (string.Equals(message.Text, command.Name, StringComparison.OrdinalIgnoreCase))
                await command.ExecuteAsync(client, message);
    }
}
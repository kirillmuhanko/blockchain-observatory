using Telegram.Bot;
using Telegram.Bot.Types;
using WorkerService.Commands.Interfaces;

namespace WorkerService.Commands.Implementations;

public class PingBotCommand : IBotCommand
{
    public string Name => "/ping";

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Pong!");
    }
}
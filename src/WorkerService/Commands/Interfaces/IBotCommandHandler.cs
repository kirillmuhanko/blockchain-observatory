using Telegram.Bot;
using Telegram.Bot.Types;

namespace WorkerService.Commands.Interfaces;

public interface IBotCommandHandler
{
    Task HandleCommandAsync(ITelegramBotClient client, Message message);
}
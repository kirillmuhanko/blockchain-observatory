using Telegram.Bot;
using Telegram.Bot.Types;
using WorkerService.Commands.Interfaces;
using WorkerService.Models;
using WorkerService.Repositories.Interfaces;

namespace WorkerService.Commands.Implementations;

public class StartBotCommand(IUserRepository userRepository) : IBotCommand
{
    public string Name => "/start";

    public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
    {
        var fromUser = message.From ?? new User();

        var userModel = new UserJsonModel
        {
            ChatId = message.Chat.Id,
            UserId = fromUser.Id,
            Username = fromUser.Username,
            FirstName = fromUser.FirstName,
            LastName = fromUser.LastName,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddUserAsync(userModel);
        var responseMessage = $"Welcome, {message.From?.FirstName}!";
        await botClient.SendTextMessageAsync(userModel.ChatId, responseMessage);
    }
}
using WorkerService.Models;

namespace WorkerService.Services.Interfaces;

public interface ITelegramAlertService
{
    Task SendMessagesAsync(IEnumerable<UserJsonModel> users, IEnumerable<string> messages);
}
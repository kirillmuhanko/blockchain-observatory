using WorkerService.Models;

namespace WorkerService.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserJsonModel>> GetUsersAsync();

    Task AddUserAsync(UserJsonModel user);
}
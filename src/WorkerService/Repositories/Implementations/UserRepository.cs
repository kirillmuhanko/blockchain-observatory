using System.Collections.Concurrent;
using System.Text.Json;
using WorkerService.Models;
using WorkerService.Repositories.Interfaces;

namespace WorkerService.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private static ConcurrentDictionary<long, UserJsonModel> _userDictionary = new();
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");

    public UserRepository()
    {
        LoadUsersFromFile();
    }

    public async Task<IEnumerable<UserJsonModel>> GetUsersAsync()
    {
        return await Task.FromResult(_userDictionary.Values);
    }

    public async Task AddUserAsync(UserJsonModel user)
    {
        ArgumentNullException.ThrowIfNull(user);
        await _fileLock.WaitAsync();

        try
        {
            _userDictionary.AddOrUpdate(user.UserId, user, (_, _) => user);
            await SaveUsersToFileAsync();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private Task SaveUsersToFileAsync()
    {
        var json = JsonSerializer.Serialize(_userDictionary);
        return File.WriteAllTextAsync(_filePath, json);
    }

    private void LoadUsersFromFile()
    {
        if (!File.Exists(_filePath))
            return;

        var json = File.ReadAllText(_filePath);
        var users = JsonSerializer.Deserialize<Dictionary<long, UserJsonModel>>(json);

        if (users != null)
            _userDictionary = new ConcurrentDictionary<long, UserJsonModel>(users);
    }
}
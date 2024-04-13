namespace WorkerService.Services.Interfaces;

public interface IInternetConnectivityService
{
    Task<bool> CheckInternetConnectionAsync(int retryDelayInMilliseconds = 60000);
}
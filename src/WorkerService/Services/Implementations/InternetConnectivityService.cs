using System.Net.NetworkInformation;
using WorkerService.Services.Interfaces;

namespace WorkerService.Services.Implementations;

public class InternetConnectivityService : IInternetConnectivityService
{
    private const string GoogleDns = "8.8.8.8";

    public async Task<bool> CheckInternetConnectionAsync(int retryDelayInMilliseconds = 60000)
    {
        var hasConnection = await PingAsync(GoogleDns);

        if (!hasConnection)
            await Task.Delay(retryDelayInMilliseconds);

        return hasConnection;
    }

    private static async Task<bool> PingAsync(string host)
    {
        try
        {
            using var ping = new Ping();
            var pingReply = await ping.SendPingAsync(host);
            return pingReply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}
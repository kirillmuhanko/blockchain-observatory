using WorkerService.Models;

namespace WorkerService.Services.Interfaces;

public interface ICryptoPriceService
{
    Task<IEnumerable<PriceMovementModel>> FetchLatestPriceMovementsAsync();
}
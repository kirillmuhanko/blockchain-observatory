using WorkerService.Models;

namespace WorkerService.Repositories.Interfaces;

public interface IPriceChangeRepository
{
    IEnumerable<string> GetPriceChangeAlerts(IEnumerable<PriceMovementModel> priceMovements);
}
using BikeRentalAPI.Models;

namespace BikeRentalAPI.Repositories.Interfaces
{
    public interface IRentalStatusRepository : IRepository<RentalStatus>
    {
        Task<RentalStatus?> GetByNameAsync(string name);
    }
}

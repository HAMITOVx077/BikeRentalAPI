using BikeRentalAPI.Models;

namespace BikeRentalAPI.Repositories.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Rental>> GetByBikeIdAsync(int bikeId);
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalsByStatusAsync(int statusId);
    }
}

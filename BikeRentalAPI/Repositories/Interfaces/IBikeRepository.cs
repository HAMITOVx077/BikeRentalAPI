using BikeRentalAPI.Models;

namespace BikeRentalAPI.Repositories.Interfaces
{
    public interface IBikeRepository : IRepository<Bike>
    {
        Task<IEnumerable<Bike>> GetAvailableBikesAsync();
        Task<IEnumerable<Bike>> GetByTypeAsync(string type);
        Task<bool> IsBikeAvailableAsync(int bikeId);
    }
}

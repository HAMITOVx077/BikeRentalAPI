using BikeRentalAPI.Models;

namespace BikeRentalAPI.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
    }
}

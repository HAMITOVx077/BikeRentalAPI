using BikeRentalAPI.Models;

namespace BikeRentalAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByLoginAsync(string login);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> LoginExistsAsync(string login);
        Task<bool> EmailExistsAsync(string email);
    }
}

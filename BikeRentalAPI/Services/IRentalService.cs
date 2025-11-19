using BikeRentalAPI.Models;

namespace BikeRentalAPI.Services
{
    public interface IRentalService
    {
        /// <summary>
        /// Получить все аренды
        /// </summary>
        Task<IEnumerable<Rental>> GetAllRentalsAsync();

        /// <summary>
        /// Получить аренду по ID
        /// </summary>
        Task<Rental?> GetRentalByIdAsync(int id);

        /// <summary>
        /// Получить активные аренды
        /// </summary>
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();

        /// <summary>
        /// Получить аренды пользователя
        /// </summary>
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId);

        /// <summary>
        /// Создать новую аренду
        /// </summary>
        Task<Rental> CreateRentalAsync(Rental rental);

        /// <summary>
        /// Завершить аренду
        /// </summary>
        Task<decimal> CompleteRentalAsync(int rentalId);

        /// <summary>
        /// Удалить аренду
        /// </summary>
        Task<bool> DeleteRentalAsync(int id);
    }
}
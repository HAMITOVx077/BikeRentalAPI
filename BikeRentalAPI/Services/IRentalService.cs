using BikeRentalAPI.Models.DTO;

namespace BikeRentalAPI.Services
{
    public interface IRentalService
    {
        /// <summary>
        /// Получить все аренды
        /// </summary>
        Task<IEnumerable<RentalDTO>> GetAllRentalsAsync();

        /// <summary>
        /// Получить аренду по ID
        /// </summary>
        Task<RentalDTO?> GetRentalByIdAsync(int id);

        /// <summary>
        /// Получить активные аренды
        /// </summary>
        Task<IEnumerable<RentalDTO>> GetActiveRentalsAsync();

        /// <summary>
        /// Получить аренды пользователя
        /// </summary>
        Task<IEnumerable<RentalDTO>> GetRentalsByUserIdAsync(int userId);

        /// <summary>
        /// Создать новую аренду
        /// </summary>
        Task<RentalDTO> CreateRentalAsync(CreateRentalDTO createRentalDto);

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
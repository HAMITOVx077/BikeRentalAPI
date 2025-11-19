using BikeRentalAPI.Models;

namespace BikeRentalAPI.Services
{
    public interface IBikeService
    {
        /// <summary>
        /// Получить все велосипеды
        /// </summary>
        Task<IEnumerable<Bike>> GetAllBikesAsync();

        /// <summary>
        /// Получить велосипед по ID
        /// </summary>
        Task<Bike?> GetBikeByIdAsync(int id);

        /// <summary>
        /// Получить доступные для аренды велосипеды
        /// </summary>
        Task<IEnumerable<Bike>> GetAvailableBikesAsync();

        /// <summary>
        /// Создать новый велосипед
        /// </summary>
        Task<Bike> CreateBikeAsync(Bike bike);

        /// <summary>
        /// Обновить данные велосипеда
        /// </summary>
        Task<Bike> UpdateBikeAsync(int id, Bike bike);

        /// <summary>
        /// Удалить велосипед
        /// </summary>
        Task<bool> DeleteBikeAsync(int id);
    }
}
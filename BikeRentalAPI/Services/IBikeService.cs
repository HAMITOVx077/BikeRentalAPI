using BikeRentalAPI.Models.DTO;

namespace BikeRentalAPI.Services
{
    public interface IBikeService
    {
        /// <summary>
        /// Получить все велосипеды
        /// </summary>
        Task<IEnumerable<BikeDTO>> GetAllBikesAsync();

        /// <summary>
        /// Получить велосипед по ID
        /// </summary>
        Task<BikeDTO?> GetBikeByIdAsync(int id);

        /// <summary>
        /// Получить доступные для аренды велосипеды
        /// </summary>
        Task<IEnumerable<BikeDTO>> GetAvailableBikesAsync();

        /// <summary>
        /// Создать новый велосипед
        /// </summary>
        Task<BikeDTO> CreateBikeAsync(CreateBikeDTO createBikeDto);

        /// <summary>
        /// Обновить данные велосипеда
        /// </summary>
        Task<BikeDTO> UpdateBikeAsync(int id, UpdateBikeDTO updateBikeDto);

        /// <summary>
        /// Удалить велосипед
        /// </summary>
        Task<bool> DeleteBikeAsync(int id);
    }
}

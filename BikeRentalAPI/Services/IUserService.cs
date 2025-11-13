using BikeRentalAPI.Models.DTO;

namespace BikeRentalAPI.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        Task<UserDTO?> GetUserByIdAsync(int id);

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto);

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO updateUserDto);

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}

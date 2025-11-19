using BikeRentalAPI.Models;

namespace BikeRentalAPI.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        Task<User> UpdateUserAsync(int id, User user);

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        Task<bool> DeleteUserAsync(int id);
    }
}
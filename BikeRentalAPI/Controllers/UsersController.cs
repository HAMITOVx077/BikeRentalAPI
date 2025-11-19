using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BikeRentalAPI.Services;
using BikeRentalAPI.Models.DTO;
using BikeRentalAPI.Models;
using AutoMapper;

namespace BikeRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")] //по умолчанию все методы только для админов
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDtos);
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Пользователь с ID {id} не найден.",
                    instance = $"/api/users/{id}"
                });

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Получить информацию о текущем пользователе
        /// </summary>
        [HttpGet("me")]
        [Authorize(Roles = "admin,user")] //для всех авторизованных
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(currentUserId);

            if (user == null)
                return NotFound();

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        [HttpPost]
        [AllowAnonymous] //регистрация доступна без авторизации
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            var createdUser = await _userService.CreateUserAsync(user);
            var userDto = _mapper.Map<UserDTO>(createdUser);

            return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, UpdateUserDTO updateUserDto)
        {
            var user = _mapper.Map<User>(updateUserDto);
            var updatedUser = await _userService.UpdateUserAsync(id, user);

            if (updatedUser == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Пользователь с ID {id} не найден.",
                    instance = $"/api/users/{id}"
                });

            var userDto = _mapper.Map<UserDTO>(updatedUser);
            return Ok(userDto);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult> DeleteUser(int id)
        {
            //сначала получаем информацию о пользователе
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Пользователь с ID {id} не найден.",
                    instance = $"/api/users/{id}"
                });

            //сохраняем информацию перед удалением
            var deletedUserInfo = new
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                DeletedAt = DateTime.UtcNow
            };

            //удаляем пользователя
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = $"Не удалось удалить пользователя с ID {id}",
                    instance = $"/api/users/{id}"
                });

            //возвращаем информацию об удаленном пользователе
            return Ok(new
            {
                message = "Пользователь успешно удален",
                deletedUser = deletedUserInfo
            });
        }
    }
}
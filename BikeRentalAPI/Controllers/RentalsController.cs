using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BikeRentalAPI.Services;
using BikeRentalAPI.Models.DTO;
using BikeRentalAPI.Models;
using AutoMapper;
using System.Security.Claims;

namespace BikeRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] //все методы требуют авторизации по умолчанию
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService rentalService, IMapper mapper)
        {
            _rentalService = rentalService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все аренды
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<IEnumerable<RentalDTO>>> GetRentals()
        {
            var rentals = await _rentalService.GetAllRentalsAsync();
            var rentalDtos = _mapper.Map<IEnumerable<RentalDTO>>(rentals);
            return Ok(rentalDtos);
        }

        /// <summary>
        /// Получить аренду по ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")] //для админов и пользователей
        public async Task<ActionResult<RentalDTO>> GetRental(int id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Аренда с ID {id} не найден.",
                    instance = $"/api/rentals/{id}"
                });

            //проверяем, что пользователь имеет доступ к этой аренде
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "admin" && rental.UserId != currentUserId)
                return Forbid();

            var rentalDto = _mapper.Map<RentalDTO>(rental);
            return Ok(rentalDto);
        }

        /// <summary>
        /// Получить активные аренды
        /// </summary>
        [HttpGet("active")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<IEnumerable<RentalDTO>>> GetActiveRentals()
        {
            var rentals = await _rentalService.GetActiveRentalsAsync();
            var rentalDtos = _mapper.Map<IEnumerable<RentalDTO>>(rentals);
            return Ok(rentalDtos);
        }

        /// <summary>
        /// Получить аренды пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "admin,user")] //для админов и пользователей
        public async Task<ActionResult<IEnumerable<RentalDTO>>> GetUserRentals(int userId)
        {
            //проверяем, что пользователь имеет доступ к этим арендам
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "admin" && userId != currentUserId)
                return Forbid();

            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            var rentalDtos = _mapper.Map<IEnumerable<RentalDTO>>(rentals);
            return Ok(rentalDtos);
        }

        /// <summary>
        /// Начать аренду велосипеда
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin,user")] //для админов и пользователей
        public async Task<ActionResult<RentalDTO>> CreateRental(CreateRentalDTO createRentalDto)
        {
            //если пользователь не админ, он может арендовать только для себя
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "admin" && createRentalDto.UserId != currentUserId)
                return Forbid();

            var rental = _mapper.Map<Rental>(createRentalDto);
            var createdRental = await _rentalService.CreateRentalAsync(rental);
            var rentalDto = _mapper.Map<RentalDTO>(createdRental);

            return CreatedAtAction(nameof(GetRental), new { id = rentalDto.Id }, rentalDto);
        }

        /// <summary>
        /// Завершить аренду и получить стоимость
        /// </summary>
        [HttpPost("{id}/complete")]
        [Authorize(Roles = "admin,user")] //для админов и пользователей
        public async Task<ActionResult<decimal>> CompleteRental(int id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null)
                return NotFound();

            //проверяем, что пользователь имеет доступ к этой аренде
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "admin" && rental.UserId != currentUserId)
                return Forbid();

            var totalPrice = await _rentalService.CompleteRentalAsync(id);
            return Ok(new { TotalPrice = totalPrice });
        }

        /// <summary>
        /// Удалить аренду
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult> DeleteRental(int id)
        {
            //сначала получаем информацию об аренде
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Аренда с ID {id} не найден.",
                    instance = $"/api/rentals/{id}"
                });

            //сохраняем информацию перед удалением
            var deletedRentalInfo = new
            {
                Id = rental.Id,
                UserId = rental.UserId,
                BikeId = rental.BikeId,
                StartTime = rental.StartTime,
                EndTime = rental.EndTime,
                TotalCost = rental.TotalCost,
                RentalStatusId = rental.RentalStatusId,
                DeletedAt = DateTime.UtcNow
            };

            //удаляем аренду
            var result = await _rentalService.DeleteRentalAsync(id);
            if (!result)
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = $"Не удалось удалить аренду с ID {id}",
                    instance = $"/api/rentals/{id}"
                });

            //возвращаем информацию об удаленной аренде
            return Ok(new
            {
                message = "Аренда успешно удалена",
                deletedRental = deletedRentalInfo
            });
        }
    }
}
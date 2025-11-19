using Microsoft.AspNetCore.Mvc;
using BikeRentalAPI.Services;
using BikeRentalAPI.Models.DTO;
using BikeRentalAPI.Models;
using AutoMapper;

namespace BikeRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

            var rentalDto = _mapper.Map<RentalDTO>(rental);
            return Ok(rentalDto);
        }

        /// <summary>
        /// Получить активные аренды
        /// </summary>
        [HttpGet("active")]
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
        public async Task<ActionResult<IEnumerable<RentalDTO>>> GetUserRentals(int userId)
        {
            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            var rentalDtos = _mapper.Map<IEnumerable<RentalDTO>>(rentals);
            return Ok(rentalDtos);
        }

        /// <summary>
        /// Начать аренду велосипеда
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RentalDTO>> CreateRental(CreateRentalDTO createRentalDto)
        {
            var rental = _mapper.Map<Rental>(createRentalDto);
            var createdRental = await _rentalService.CreateRentalAsync(rental);
            var rentalDto = _mapper.Map<RentalDTO>(createdRental);

            return CreatedAtAction(nameof(GetRental), new { id = rentalDto.Id }, rentalDto);
        }

        /// <summary>
        /// Завершить аренду и получить стоимость
        /// </summary>
        [HttpPost("{id}/complete")]
        public async Task<ActionResult<decimal>> CompleteRental(int id)
        {
            var totalPrice = await _rentalService.CompleteRentalAsync(id);
            return Ok(new { TotalPrice = totalPrice });
        }

        /// <summary>
        /// Удалить аренду
        /// </summary>
        [HttpDelete("{id}")]
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
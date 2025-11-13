using Microsoft.AspNetCore.Mvc;
using BikeRentalAPI.Services;
using BikeRentalAPI.Models.DTO;
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
            return Ok(rentals);
        }

        /// <summary>
        /// Получить аренду по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDTO>> GetRental(int id)
        {
            var rental = await _rentalService.GetRentalByIdAsync(id);
            if (rental == null)
                return NotFound();
            return Ok(rental);
        }

        /// <summary>
        /// Получить активные аренды
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<RentalDTO>>> GetActiveRentals()
        {
            var rentals = await _rentalService.GetActiveRentalsAsync();
            return Ok(rentals);
        }

        /// <summary>
        /// Получить аренды пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<RentalDTO>>> GetUserRentals(int userId)
        {
            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            return Ok(rentals);
        }

        /// <summary>
        /// Начать аренду велосипеда
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RentalDTO>> CreateRental(CreateRentalDTO createRentalDto)
        {
            try
            {
                var rental = await _rentalService.CreateRentalAsync(createRentalDto);
                return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Завершить аренду и получить стоимость
        /// </summary>
        [HttpPost("{id}/complete")]
        public async Task<ActionResult<decimal>> CompleteRental(int id)
        {
            try
            {
                var totalPrice = await _rentalService.CompleteRentalAsync(id);
                return Ok(new { TotalPrice = totalPrice });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить аренду
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRental(int id)
        {
            try
            {
                await _rentalService.DeleteRentalAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
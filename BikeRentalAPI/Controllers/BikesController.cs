using Microsoft.AspNetCore.Mvc;
using BikeRentalAPI.Services;
using BikeRentalAPI.Models.DTO;
using AutoMapper;

namespace BikeRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BikesController : ControllerBase
    {
        private readonly IBikeService _bikeService;
        private readonly IMapper _mapper;

        public BikesController(IBikeService bikeService, IMapper mapper)
        {
            _bikeService = bikeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все велосипеды
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeDTO>>> GetBikes()
        {
            var bikes = await _bikeService.GetAllBikesAsync();
            return Ok(bikes);
        }

        /// <summary>
        /// Получить велосипед по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeDTO>> GetBike(int id)
        {
            var bike = await _bikeService.GetBikeByIdAsync(id);
            if (bike == null)
                return NotFound();
            return Ok(bike);
        }

        /// <summary>
        /// Получить доступные велосипеды
        /// </summary>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<BikeDTO>>> GetAvailableBikes()
        {
            var bikes = await _bikeService.GetAvailableBikesAsync();
            return Ok(bikes);
        }

        /// <summary>
        /// Создать новый велосипед
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BikeDTO>> CreateBike(CreateBikeDTO createBikeDto)
        {
            try
            {
                var bike = await _bikeService.CreateBikeAsync(createBikeDto);
                return CreatedAtAction(nameof(GetBike), new { id = bike.Id }, bike);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновить велосипед
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BikeDTO>> UpdateBike(int id, UpdateBikeDTO updateBikeDto)
        {
            try
            {
                var bike = await _bikeService.UpdateBikeAsync(id, updateBikeDto);
                return Ok(bike);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удалить велосипед
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBike(int id)
        {
            try
            {
                await _bikeService.DeleteBikeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
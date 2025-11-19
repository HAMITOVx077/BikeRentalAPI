using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BikeRentalAPI.Services;
using BikeRentalAPI.Models.DTO;
using BikeRentalAPI.Models;
using AutoMapper;

namespace BikeRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [AllowAnonymous] //доступно без авторизации
        public async Task<ActionResult<IEnumerable<BikeDTO>>> GetBikes()
        {
            var bikes = await _bikeService.GetAllBikesAsync();
            var bikeDtos = _mapper.Map<IEnumerable<BikeDTO>>(bikes);
            return Ok(bikeDtos);
        }

        /// <summary>
        /// Получить велосипед по ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous] //доступно без авторизации
        public async Task<ActionResult<BikeDTO>> GetBike(int id)
        {
            var bike = await _bikeService.GetBikeByIdAsync(id);
            if (bike == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Велосипед с ID {id} не найден.",
                    instance = $"/api/bikes/{id}"
                });

            var bikeDto = _mapper.Map<BikeDTO>(bike);
            return Ok(bikeDto);
        }

        /// <summary>
        /// Получить доступные велосипеды
        /// </summary>
        [HttpGet("available")]
        [AllowAnonymous] //доступно без авторизации
        public async Task<ActionResult<IEnumerable<BikeDTO>>> GetAvailableBikes()
        {
            var bikes = await _bikeService.GetAvailableBikesAsync();
            var bikeDtos = _mapper.Map<IEnumerable<BikeDTO>>(bikes);
            return Ok(bikeDtos);
        }

        /// <summary>
        /// Создать новый велосипед
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<BikeDTO>> CreateBike(CreateBikeDTO createBikeDto)
        {
            var bike = _mapper.Map<Bike>(createBikeDto);
            var createdBike = await _bikeService.CreateBikeAsync(bike);
            var bikeDto = _mapper.Map<BikeDTO>(createdBike);

            return CreatedAtAction(nameof(GetBike), new { id = bikeDto.Id }, bikeDto);
        }

        /// <summary>
        /// Обновить велосипед
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult<BikeDTO>> UpdateBike(int id, UpdateBikeDTO updateBikeDto)
        {
            var bike = _mapper.Map<Bike>(updateBikeDto);
            var updatedBike = await _bikeService.UpdateBikeAsync(id, bike);

            if (updatedBike == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Велосипед с ID {id} не найден.",
                    instance = $"/api/bikes/{id}"
                });

            var bikeDto = _mapper.Map<BikeDTO>(updatedBike);
            return Ok(bikeDto);
        }

        /// <summary>
        /// Удалить велосипед
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] //только для админов
        public async Task<ActionResult> DeleteBike(int id)
        {
            //сначала получаем информацию о велосипеде
            var bike = await _bikeService.GetBikeByIdAsync(id);
            if (bike == null)
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"Велосипед с ID {id} не найден.",
                    instance = $"/api/bikes/{id}"
                });

            //сохраняем информацию перед удалением
            var deletedBikeInfo = new
            {
                Id = bike.Id,
                Model = bike.Model,
                PricePerHour = bike.PricePerHour,
                DeletedAt = DateTime.UtcNow
            };

            //удаляем велосипед
            var result = await _bikeService.DeleteBikeAsync(id);
            if (!result)
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = 400,
                    detail = $"Не удалось удалить велосипед с ID {id}",
                    instance = $"/api/bikes/{id}"
                });

            //возвращаем информацию об удаленном велосипеде
            return Ok(new
            {
                message = "Велосипед успешно удален",
                deletedBike = deletedBikeInfo
            });
        }
    }
}
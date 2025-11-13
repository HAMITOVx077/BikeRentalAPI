using AutoMapper;
using BikeRentalAPI.Models.DTO;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Services
{
    public class BikeService : IBikeService
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IMapper _mapper;

        public BikeService(IBikeRepository bikeRepository, IMapper mapper)
        {
            _bikeRepository = bikeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все велосипеды
        /// </summary>
        public async Task<IEnumerable<BikeDTO>> GetAllBikesAsync()
        {
            var bikes = await _bikeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BikeDTO>>(bikes);
        }

        /// <summary>
        /// Получить велосипед по ID
        /// </summary>
        public async Task<BikeDTO?> GetBikeByIdAsync(int id)
        {
            var bike = await _bikeRepository.GetByIdAsync(id);
            return _mapper.Map<BikeDTO>(bike);
        }

        /// <summary>
        /// Получить доступные для аренды велосипеды
        /// </summary>
        public async Task<IEnumerable<BikeDTO>> GetAvailableBikesAsync()
        {
            var bikes = await _bikeRepository.GetAvailableBikesAsync();
            return _mapper.Map<IEnumerable<BikeDTO>>(bikes);
        }

        /// <summary>
        /// Создать новый велосипед
        /// </summary>
        public async Task<BikeDTO> CreateBikeAsync(CreateBikeDTO createBikeDto)
        {
            var bike = _mapper.Map<Bike>(createBikeDto);
            bike.CreatedAt = DateTime.UtcNow;

            var createdBike = await _bikeRepository.CreateAsync(bike);
            return _mapper.Map<BikeDTO>(createdBike);
        }

        /// <summary>
        /// Обновить данные велосипеда
        /// </summary>
        public async Task<BikeDTO> UpdateBikeAsync(int id, UpdateBikeDTO updateBikeDto)
        {
            var existingBike = await _bikeRepository.GetByIdAsync(id);
            if (existingBike == null)
                return null;

            _mapper.Map(updateBikeDto, existingBike);
            var updatedBike = await _bikeRepository.UpdateAsync(existingBike);
            return _mapper.Map<BikeDTO>(updatedBike);
        }

        /// <summary>
        /// Удалить велосипед
        /// </summary>
        public async Task<bool> DeleteBikeAsync(int id)
        {
            return await _bikeRepository.DeleteAsync(id);
        }
    }
}

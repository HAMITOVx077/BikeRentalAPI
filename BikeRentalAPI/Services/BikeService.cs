using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Services
{
    public class BikeService : IBikeService
    {
        private readonly IBikeRepository _bikeRepository;

        public BikeService(IBikeRepository bikeRepository)
        {
            _bikeRepository = bikeRepository;
        }

        public async Task<IEnumerable<Bike>> GetAllBikesAsync()
        {
            return await _bikeRepository.GetAllAsync();
        }

        public async Task<Bike?> GetBikeByIdAsync(int id)
        {
            return await _bikeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Bike>> GetAvailableBikesAsync()
        {
            return await _bikeRepository.GetAvailableBikesAsync();
        }

        public async Task<Bike> CreateBikeAsync(Bike bike)
        {
            bike.CreatedAt = DateTime.UtcNow;
            return await _bikeRepository.CreateAsync(bike);
        }

        public async Task<Bike> UpdateBikeAsync(int id, Bike bike)
        {
            var existingBike = await _bikeRepository.GetByIdAsync(id);
            if (existingBike == null)
                return null; 

            existingBike.Model = bike.Model;
            existingBike.PricePerHour = bike.PricePerHour;
            existingBike.IsAvailable = bike.IsAvailable;

            return await _bikeRepository.UpdateAsync(existingBike);
        }

        public async Task<bool> DeleteBikeAsync(int id)
        {
            return await _bikeRepository.DeleteAsync(id);
        }
    }
}
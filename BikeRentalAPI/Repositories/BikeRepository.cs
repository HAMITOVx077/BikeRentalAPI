using Microsoft.EntityFrameworkCore;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private readonly APIDBContext _context;

        public BikeRepository(APIDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bike>> GetAllAsync()
        {
            return await _context.Bikes.ToListAsync();
        }

        public async Task<Bike?> GetByIdAsync(int id)
        {
            return await _context.Bikes.FindAsync(id);
        }

        public async Task<Bike> CreateAsync(Bike entity)
        {
            await _context.Bikes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Bike> UpdateAsync(Bike entity)
        {
            _context.Bikes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bike = await GetByIdAsync(id);
            if (bike == null)
                return false;

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bikes.AnyAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bike>> GetAvailableBikesAsync()
        {
            return await _context.Bikes.Where(b => b.IsAvailable).ToListAsync();
        }

        public async Task<IEnumerable<Bike>> GetByTypeAsync(string type)
        {
            return await _context.Bikes.Where(b => b.Model.Contains(type)).ToListAsync();
        }

        public async Task<bool> IsBikeAvailableAsync(int bikeId)
        {
            var bike = await GetByIdAsync(bikeId);
            return bike?.IsAvailable ?? false;
        }
    }
}
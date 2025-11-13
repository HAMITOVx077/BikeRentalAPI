using Microsoft.EntityFrameworkCore;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly APIDBContext _context;

        public RentalRepository(APIDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Include(r => r.RentalStatus)
                .ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Include(r => r.RentalStatus)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Rental> CreateAsync(Rental entity)
        {
            await _context.Rentals.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Rental> UpdateAsync(Rental entity)
        {
            _context.Rentals.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rental = await GetByIdAsync(id);
            if (rental == null)
                return false;

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rentals.AnyAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rental>> GetByUserIdAsync(int userId)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Include(r => r.RentalStatus)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetByBikeIdAsync(int bikeId)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Include(r => r.RentalStatus)
                .Where(r => r.BikeId == bikeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Include(r => r.RentalStatus)
                .Where(r => r.RentalStatusId == 1) //Active
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByStatusAsync(int statusId)
        {
            return await _context.Rentals
                .Include(r => r.User)
                .Include(r => r.Bike)
                .Include(r => r.RentalStatus)
                .Where(r => r.RentalStatusId == statusId)
                .ToListAsync();
        }
    }
}
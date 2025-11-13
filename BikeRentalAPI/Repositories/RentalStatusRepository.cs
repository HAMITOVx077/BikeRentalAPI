using Microsoft.EntityFrameworkCore;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Repositories
{
    public class RentalStatusRepository : IRentalStatusRepository
    {
        private readonly APIDBContext _context;

        public RentalStatusRepository(APIDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentalStatus>> GetAllAsync()
        {
            return await _context.RentalStatuses.ToListAsync();
        }

        public async Task<RentalStatus?> GetByIdAsync(int id)
        {
            return await _context.RentalStatuses.FindAsync(id);
        }

        public async Task<RentalStatus> CreateAsync(RentalStatus entity)
        {
            await _context.RentalStatuses.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<RentalStatus> UpdateAsync(RentalStatus entity)
        {
            _context.RentalStatuses.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await GetByIdAsync(id);
            if (status == null)
                return false;

            _context.RentalStatuses.Remove(status);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RentalStatuses.AnyAsync(rs => rs.Id == id);
        }

        public async Task<RentalStatus?> GetByNameAsync(string name)
        {
            return await _context.RentalStatuses.FirstOrDefaultAsync(rs => rs.Name == name);
        }
    }
}
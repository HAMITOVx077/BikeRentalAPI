using Microsoft.EntityFrameworkCore;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly APIDBContext _context;

        public RoleRepository(APIDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> CreateAsync(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Role> UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);
            if (role == null)
                return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly APIDBContext _context;

        public UserRepository(APIDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
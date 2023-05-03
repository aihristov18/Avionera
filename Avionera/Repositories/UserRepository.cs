using Avionera.Data;
using Avionera.Interfaces;
using Avionera.Models;
using Microsoft.EntityFrameworkCore;

namespace Avionera.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _dbContext.Users
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _dbContext.Users
                .Include(u => u.UsersOffers)
                .ThenInclude(uO => uO.Offer)
                .SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}

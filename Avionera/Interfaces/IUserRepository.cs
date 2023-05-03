using Avionera.Models;

namespace Avionera.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<AppUser>> GetAllAsync();
        public Task<AppUser> GetUserByIdAsync(string id);
    }
}

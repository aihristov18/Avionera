using Avionera.Models;

namespace Avionera.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IOfferRepository Offers { get; }
        IUserOfferRepository UserOffers { get; }
        Task<int> SaveChangesAsync();
    }
}

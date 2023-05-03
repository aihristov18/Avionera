using Avionera.Interfaces;
using Avionera.Models;
using Avionera.Repositories;

namespace Avionera.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IUserRepository Users { get; }
        public IOfferRepository Offers { get; }
        public IUserOfferRepository UserOffers { get; }

        public UnitOfWork(ApplicationDbContext dbContext, IUserRepository users, IOfferRepository offers, IUserOfferRepository userOffer)
        {
            _dbContext = dbContext;
            Users = users;
            Offers = offers;
            UserOffers = userOffer;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

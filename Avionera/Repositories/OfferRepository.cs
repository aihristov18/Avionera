using Avionera.Data;
using Avionera.Interfaces;
using Avionera.Models;
using Microsoft.EntityFrameworkCore;

namespace Avionera.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OfferRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Offer>> GetAllAsync()
        {
            return await _dbContext.Offers
                .Include(offer => offer.OffersUsers)
                .ThenInclude(offerUsers => offerUsers.User)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllBookmarkedAsync(AppUser user)
        {
            return await _dbContext.Offers
                .Where(x => x.OffersUsers.Any(o => (o.UserId == user.Id && o.IsBookmarked) || (o.UserId==user.Id && o.IsCreatedByUser)) && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Offer> GetOfferByIdAsync(int id)
        {
            return await _dbContext.Offers
                .Include(offer => offer.OffersUsers)
                .ThenInclude(offerUsers => offerUsers.User)
                .Where(b => b.OfferId == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddOfferAsync(Offer offer)
        {
            await _dbContext.Offers.AddAsync(offer);
        }
        public async Task<AppUser> GetOfferCreatorAsync(int offerId)
        {
            var offer = await _dbContext.Offers
                .Include(o => o.OffersUsers)
                    .ThenInclude(uo => uo.User)
                .FirstOrDefaultAsync(o => o.OfferId == offerId);

            var creator = offer.OffersUsers
                .FirstOrDefault(uo => uo.IsCreatedByUser)?.User;

            return creator;
        }
        public Task<List<Offer>> SearchOffersAsync(string query)
        {
            return _dbContext.Offers
                .Where(p => (p.Title.Contains(query) || p.Description.Contains(query) || p.LocationName.Contains(query)) && !p.IsDeleted)
                .ToListAsync();
        }
    }
}

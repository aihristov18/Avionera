using Avionera.Data;
using Avionera.Interfaces;
using Avionera.Models;
using Microsoft.EntityFrameworkCore;

namespace Avionera.Repositories
{
    public class UserOfferRepository : IUserOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public UserOfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBookmarkedOfferCreatorAsync(AppUser user, Offer offer)
        {
            await _context.AppUsersOffers.AddAsync(new AppUserOffer()
            {
                User = user,
                Offer = offer,
                IsCreatedByUser = true,
                IsBookmarked = true
            });
        }

        public async Task<bool> IsUserAlreadyBookmarkedAsync(string userId, int offerId)
        {
            return await _context.AppUsersOffers
                .AnyAsync(x => x.UserId == userId && x.OfferId == offerId && x.IsBookmarked);
        }
        public async Task<bool> IsUserOfferCreatorAsync(string userId, int offerId)
        {
            return await _context.AppUsersOffers
                .AnyAsync(x => x.UserId == userId && x.OfferId == offerId && x.IsCreatedByUser);
        }

        public async Task BookmarkOfferAsync(AppUser user, Offer offer)
        {
            var appOffer = await _context.AppUsersOffers
                              .FirstOrDefaultAsync(a => a.UserId == user.Id && a.OfferId == offer.OfferId);
            if (appOffer != null)
            {
                appOffer.IsBookmarked = true;
            }
            else
            {
                await _context.AppUsersOffers.AddAsync(new AppUserOffer()
                {
                    User = user,
                    Offer = offer,
                    IsCreatedByUser = false,
                    IsBookmarked = true
                });
            }
        }
        public async Task RemoveBookmarkOfferAsync(AppUser user, Offer offer)
        {
            var appOffer = await _context.AppUsersOffers
                              .FirstOrDefaultAsync(a => a.UserId == user.Id && a.OfferId == offer.OfferId && a.IsBookmarked);
            if (appOffer != null)
            {
                appOffer.IsBookmarked = false;
            }
        }
    }
}

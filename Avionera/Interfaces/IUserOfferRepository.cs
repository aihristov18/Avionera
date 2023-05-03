using Avionera.Models;
using Microsoft.EntityFrameworkCore;

namespace Avionera.Interfaces
{
    public interface IUserOfferRepository
    {
        public Task AddBookmarkedOfferCreatorAsync(AppUser user, Offer offer);
        public Task<bool> IsUserAlreadyBookmarkedAsync(string userId, int offerId);
        public Task<bool> IsUserOfferCreatorAsync(string userId, int offerId);
        public Task BookmarkOfferAsync(AppUser user, Offer offer);
        public Task RemoveBookmarkOfferAsync(AppUser user, Offer offer);
    }
}

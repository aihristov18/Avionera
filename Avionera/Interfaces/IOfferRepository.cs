using Avionera.Models;

namespace Avionera.Interfaces
{
    public interface IOfferRepository
    {
        public Task<List<Offer>> GetAllAsync();
        public Task<List<Offer>> GetAllBookmarkedAsync(AppUser user);
        public Task<Offer> GetOfferByIdAsync(int id);
        public Task AddOfferAsync(Offer offer);
        public Task<List<Offer>> SearchOffersAsync(string query);
        public Task<AppUser> GetOfferCreatorAsync(int offerId);
    }
}

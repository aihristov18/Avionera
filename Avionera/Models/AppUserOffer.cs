namespace Avionera.Models
{
    public class AppUserOffer
    {
        public string UserId { get; set; }
        public int OfferId { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsCreatedByUser { get; set; }

        public AppUser User { get; set; }
        public Offer Offer { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Avionera.ViewModels
{
    public class OfferEditViewModel
    {
        public int OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LocationName { get; set; }
        public bool IsDeleted { get; set; }
        public IFormFile Image { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

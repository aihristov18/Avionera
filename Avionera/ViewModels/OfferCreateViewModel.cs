using System.ComponentModel.DataAnnotations;

namespace Avionera.ViewModels
{
    public class OfferCreateViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string LocationName { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

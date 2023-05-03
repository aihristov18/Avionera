using Avionera.Models;
using System.ComponentModel.DataAnnotations;

namespace Avionera.ViewModels
{
    public class OfferViewModel
    {
        public int OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LocationName { get; set; }
        public AppUser Creator { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

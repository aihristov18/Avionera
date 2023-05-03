using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avionera.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public decimal Price { get; set; }
        public string LocationName { get; set; }  
        public byte[] Image { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public List<AppUserOffer> OffersUsers { get; set; }
    }
}

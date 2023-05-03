using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avionera.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string CitizenNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public ICollection<AppUserOffer> UsersOffers { get; set; }
    }
}

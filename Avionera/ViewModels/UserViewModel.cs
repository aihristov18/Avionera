using Avionera.Models;
using System.ComponentModel.DataAnnotations;

namespace Avionera.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public IFormFile ImageFF { get; set; }
        public string ImageURL { get; set; }
        [MaxLength(10)]
        public string CitizenNumber { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
    }
}

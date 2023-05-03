using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Avionera.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string CurrentRoleName { get; set; }
        public string SelectedRoleName { get; set; }

        public List<IdentityRole> AvailableRoles { get; set; }
    }
}

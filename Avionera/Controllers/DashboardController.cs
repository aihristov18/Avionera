using Avionera.Data;
using Avionera.Interfaces;
using Avionera.Models;
using Avionera.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Avionera.Controllers
{
    [Authorize(Roles = UserRoles.Administrator)]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public DashboardController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DashboardUsersViewModel dashboardViewModel)
        {
            var dashboardVM = new DashboardUsersViewModel()
            {
                Users = await _userManager.Users.ToListAsync()
            };
            return View(dashboardVM);
        }
    }
}

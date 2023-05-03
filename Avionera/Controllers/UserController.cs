using AutoMapper;
using Avionera.Data;
using Avionera.Interfaces;
using Avionera.Models;
using Avionera.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.WebSockets;
using System.Security;

namespace Avionera.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICitizenNumberValidator _citizenNumberValidator;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IImageConverter _imageConverter;

        public UserController(ICitizenNumberValidator validator, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IImageConverter imageConverter)
        {
            _citizenNumberValidator = validator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _imageConverter = imageConverter;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return View("Error");

            var userVM = _mapper.Map<UserViewModel>(user);

            if(user.ProfilePicture != null)
            {
                userVM.ImageURL = _imageConverter.ByteArrayToImgUrl(user.ProfilePicture);
            }

            return View(userVM);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile(string id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return View("Error");

            var userVM = _mapper.Map<UserViewModel>(user);
            
            return View(userVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(UserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "All fields are required!");
                return View("EditProfile", userVM);
            }
            var user = await _unitOfWork.Users.GetUserByIdAsync(userVM.Id);

            if (!_citizenNumberValidator.IsValid(userVM.CitizenNumber))
            {
                ModelState.AddModelError(string.Empty, "Citizen number is invalid!");
                return View("EditProfile", userVM);
            }

            _mapper.Map(userVM, user);
            
            if(userVM.ImageFF != null)
            {
                user.ProfilePicture = await _imageConverter.ToByteArrayAsync(userVM.ImageFF);
            }

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Cannot update user!");
                return View(userVM);
            }
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> EditProfileAdmin(string id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            if (user == null) return View("Error");

            var userVM = _mapper.Map<UserEditAdminViewModel>(user);

            return View(userVM);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> EditProfileAdmin(UserEditAdminViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "All fields are required!");
                return View(userVM);
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if(!await _userManager.CheckPasswordAsync(currentUser, userVM.AdminPassword))
            {
                ModelState.AddModelError(string.Empty, "Your password is wrong!");
                return View(userVM);
            }

            var user = await _unitOfWork.Users.GetUserByIdAsync(userVM.Id);

            if (!_citizenNumberValidator.IsValid(userVM.CitizenNumber))
            {
                ModelState.AddModelError(string.Empty, "Citizen number is invalid!");
                return View(userVM);
            }

            _mapper.Map(userVM, user);

            if (userVM.Image != null)
            {
                user.ProfilePicture = await _imageConverter.ToByteArrayAsync(userVM.Image);
            }

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(userVM);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> EditUserRole(string id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);

            var userVM = new UserRolesViewModel()
            {
                UserId = user.Id,
                CurrentRoleName = userRoles.FirstOrDefault().ToUpper(),
                AvailableRoles = _roleManager.Roles.ToList()
            };

            return View(userVM);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> EditUserRole(UserRolesViewModel roleVM)
        {
            if(ModelState.IsValid)
            {
                var user = await _unitOfWork.Users.GetUserByIdAsync(roleVM.UserId);
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.Id == user.Id)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                if (await _userManager.IsInRoleAsync(user, roleVM.CurrentRoleName))
                {
                    // Remove the user from the role
                    await _userManager.RemoveFromRoleAsync(user, roleVM.CurrentRoleName);
                }
                if (!await _userManager.IsInRoleAsync(user, roleVM.SelectedRoleName))
                {
                    // Add the user to the new role
                    await _userManager.AddToRoleAsync(user, roleVM.SelectedRoleName);
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}

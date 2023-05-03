using AutoMapper;
using Avionera.Data;
using Avionera.Interfaces;
using Avionera.Models;
using Avionera.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Avionera.Controllers
{
    public class OfferController : Controller
    {
        public IUnitOfWork _unitOfWork { get; set; }
        public UserManager<AppUser> _userManager { get; set; }
        public IImageConverter _imageConverter { get; set; }
        public IMapper _mapper { get; set; }

        public OfferController(IUnitOfWork unitOfWork, IImageConverter imageConverter, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _imageConverter = imageConverter;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> BookmarkedOffers()
        {
            var user = await _userManager.GetUserAsync(User);
            var offersVM = _mapper.Map<List<Offer>, List<OfferViewModel>>(await _unitOfWork.Offers.GetAllBookmarkedAsync(user));
            var offersBrowserVM = new OfferBrowserViewModel()
            {
                Offers = offersVM
            };
            return View(offersBrowserVM);
        }

        [HttpGet]
        public async Task<IActionResult> OfferBrowser(string searchQuery)
        {
            var offersVM = new List<OfferViewModel>();
            if (searchQuery != null)
            {
                offersVM = _mapper.Map<List<Offer>, List<OfferViewModel>>(await _unitOfWork.Offers.SearchOffersAsync(searchQuery));
            }
            else
            {
                offersVM = _mapper.Map<List<Offer>, List<OfferViewModel>>(await _unitOfWork.Offers.GetAllAsync());
            }

            var offersBrowserVM = new OfferBrowserViewModel()
            {
                Offers = offersVM
            };
            return View(offersBrowserVM);
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.TravelAgent}, {UserRoles.Administrator}")]
        public IActionResult CreateOffer()
        {
            var offerVM = new OfferCreateViewModel()
            {
                DateCreated = DateTime.Now.Date
            };
            return View(offerVM);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.TravelAgent}, {UserRoles.Administrator}")]
        public async Task<IActionResult> CreateOffer(OfferCreateViewModel offerVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "All fields are required!");
                return View(offerVM);
            }

            var user = await _userManager.GetUserAsync(User);
            var offer = new Offer()
            {
                Image = await _imageConverter.ToByteArrayAsync(offerVM.Image)
            };

            _mapper.Map(offerVM, offer);
            await _unitOfWork.Offers.AddOfferAsync(offer);
            await _unitOfWork.UserOffers.AddBookmarkedOfferCreatorAsync(user, offer);
            if(await _unitOfWork.SaveChangesAsync() > 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Something went wrong!");
            return View(offerVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _unitOfWork.Offers.GetOfferByIdAsync(id);
            var offerVM = new OfferViewModel();
            _mapper.Map(offer, offerVM);
            offerVM.Creator = await _unitOfWork.Offers.GetOfferCreatorAsync(id);
            return View(offerVM);
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.TravelAgent}, {UserRoles.Administrator}")]
        public async Task<IActionResult> EditOffer(int id)
        {
            var offer = await _unitOfWork.Offers.GetOfferByIdAsync(id);

            var offerVM = new OfferEditViewModel()
            {
                OfferId = offer.OfferId,
                DateCreated = offer.DateCreated,
                Description = offer.Description,
                Image = _imageConverter.ByteArrayToFormFile(offer.Image),
                LocationName = offer.LocationName,
                Price = offer.Price,
                Title = offer.Title
            };

            return View(offerVM);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.TravelAgent}, {UserRoles.Administrator}")]
        public async Task<IActionResult> EditOffer(OfferEditViewModel offerVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong!");
                return View(offerVM);
            }

            var offer = await _unitOfWork.Offers.GetOfferByIdAsync(offerVM.OfferId);

            offer.Description = offerVM.Description;
            offer.Price = offerVM.Price;
            offer.DateCreated = offerVM.DateCreated;
            if(offerVM.Image != null)
            {
                offer.Image = await _imageConverter.ToByteArrayAsync(offerVM.Image);
            }
            offer.Title = offerVM.Title;
            offer.LocationName = offerVM.LocationName;
            offer.IsDeleted = offerVM.IsDeleted;

            if(await _unitOfWork.SaveChangesAsync() > 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Cannot update offer!");
                return View(offerVM);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BookmarkOffer(int offerId)
        {
            var curUser = await _userManager.GetUserAsync(User);
            var offer = await _unitOfWork.Offers.GetOfferByIdAsync(offerId);
            await _unitOfWork.UserOffers.BookmarkOfferAsync(curUser, offer);
            await _unitOfWork.SaveChangesAsync();
            if (Request.Headers.ContainsKey("Referer"))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveBookmarkOffer(int offerId)
        {
            var curUser = await _userManager.GetUserAsync(User);
            var offer = await _unitOfWork.Offers.GetOfferByIdAsync(offerId);
            await _unitOfWork.UserOffers.RemoveBookmarkOfferAsync(curUser, offer);
            await _unitOfWork.SaveChangesAsync();
            if (Request.Headers.ContainsKey("Referer"))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}

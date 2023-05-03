using Avionera.Data;
using Avionera.Repositories;
using Microsoft.EntityFrameworkCore;
using Avionera.Models;

namespace Avionera.UnitTests
{
    internal class OfferRepositoryTests
    {
        private ApplicationDbContext _context;
        private OfferRepository _offerRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            _offerRepository = new OfferRepository(_context);
        }

        [Test]
        public async Task AddOffer_ValidInput_AddsOffer()
        {
            var offer = new Offer()
            {
                Title = "Test",
                Description = "Test Test Test",
                DateCreated = DateTime.Now,
                IsDeleted = false,
                LocationName = "Test",
                Price = 1,
                Image = null
            };

            await _offerRepository.AddOfferAsync(offer);
            await _context.SaveChangesAsync();

            Assert.That(_context.Offers.CountAsync().Result, Is.EqualTo(1));
        }
        [Test]
        public async Task GetAll_IsGreaterThan0()
        {
            var offer = new Offer()
            {
                Title = "Test",
                Description = "Test Test Test",
                DateCreated = DateTime.Now,
                IsDeleted = false,
                LocationName = "Test",
                Price = 1,
                Image = null
            };

            await _offerRepository.AddOfferAsync(offer);
            await _context.SaveChangesAsync();

            var result = await _offerRepository.GetAllAsync();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
        [Test]
        public async Task SearchOffers_ValidQuery_IsGreaterThan0()
        {
            var offer = new Offer()
            {
                Title = "Test",
                Description = "Test Test Test",
                DateCreated = DateTime.Now,
                IsDeleted = false,
                LocationName = "Test",
                Price = 1,
                Image = null
            };

            await _offerRepository.AddOfferAsync(offer);
            await _context.SaveChangesAsync();

            var result = await _offerRepository.SearchOffersAsync("Test");

            Assert.That(result.Count, Is.GreaterThan(0));
        }
        [Test]
        public async Task SearchOffers_InvalidQuery_IsEqualTo0()
        {
            var offer = new Offer()
            {
                Title = "Test",
                Description = "Test Test Test",
                DateCreated = DateTime.Now,
                IsDeleted = false,
                LocationName = "Test",
                Price = 1,
                Image = null
            };

            await _offerRepository.AddOfferAsync(offer);
            await _context.SaveChangesAsync();

            var result = await _offerRepository.SearchOffersAsync("Testt");

            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task GetOfferById_ValidInput_IsNotNull()
        {
            var offer = new Offer()
            {
                OfferId = 1,
                Title = "Test",
                Description = "Test Test Test",
                DateCreated = DateTime.Now,
                IsDeleted = false,
                LocationName = "Test",
                Price = 1,
                Image = null
            };

            await _offerRepository.AddOfferAsync(offer);
            await _context.SaveChangesAsync();

            var result = await _offerRepository.GetOfferByIdAsync(1);

            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public async Task GetOfferById_InvalidInput_IsNull()
        {
            var offer = new Offer()
            {
                OfferId = 1,
                Title = "Test",
                Description = "Test Test Test",
                DateCreated = DateTime.Now,
                IsDeleted = false,
                LocationName = "Test",
                Price = 1,
                Image = null
            };

            await _offerRepository.AddOfferAsync(offer);
            await _context.SaveChangesAsync();

            var result = await _offerRepository.GetOfferByIdAsync(2);

            Assert.That(result, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}

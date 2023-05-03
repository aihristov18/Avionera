using Avionera.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Avionera.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUserOffer>()
                .HasKey(o => new { o.UserId, o.OfferId });

            modelBuilder.Entity<Offer>()
                .Property(o => o.IsDeleted)
                .HasDefaultValue(false);
        }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<AppUserOffer> AppUsersOffers { get; set; }
    }
}

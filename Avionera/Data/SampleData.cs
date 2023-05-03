using Avionera.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Avionera.Data
{
    public class SampleData
    {
        public static async Task InitializeAsync(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                if (!await roleManager.RoleExistsAsync(UserRoles.TravelAgent))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.TravelAgent));
                if (!await roleManager.RoleExistsAsync(UserRoles.Administrator))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminEmail = "admin@administration.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        DateCreated = DateTime.Now,
                        CitizenNumber = "6402074986",
                        FirstName = "Admin",
                        MiddleName = "Adminski",
                        LastName = "Adminov",
                        PhoneNumber = "0884332404",
                    };
                    await userManager.CreateAsync(newAdminUser, "Admin@1");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Administrator);
                }
            }
        }
    }
}

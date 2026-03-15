using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Models;

namespace StayEase.DAL.Utilits
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "HAbuomar",
                    Email = "h@gmail.com",
                    FullName = "Hmmod Abu Omar",
                    EmailConfirmed = true
                };

                var user2 = new ApplicationUser
                {
                    UserName = "ANassar",
                    Email = "a@gmail.com",
                    FullName = "Abdallah Nassar",
                    EmailConfirmed = true
                };

                var user3 = new ApplicationUser
                {
                    UserName = "BQassarwi.com",
                    Email = "b@gmail.com",
                    FullName = "Basil Qassarwi",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user1, "Hmood@44");
                await _userManager.CreateAsync(user2, "Hmood@44");
                await _userManager.CreateAsync(user3, "Hmood@44");

                await _userManager.AddToRoleAsync(user1, "Admin");
                await _userManager.AddToRoleAsync(user2, "User");
                await _userManager.AddToRoleAsync(user3, "User");

            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StayEase.DAL.Utilits
{
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task DataSeed()
        {
            string[] roles = { "Admin", "User" };
            if (!await _roleManager.Roles.AnyAsync())
            {
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}

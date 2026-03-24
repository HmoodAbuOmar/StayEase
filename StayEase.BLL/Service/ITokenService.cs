using StayEase.DAL.Models;
using System.Security.Claims;

namespace StayEase.BLL.Service
{
    public interface ITokenService
    {
        public Task<string> GenerateAccessToken(ApplicationUser user);

        string GeneratedRefreshToken();

        ClaimsPrincipal GetPrincipalsFormExpiredToken(string token);
    }
}

using Microsoft.AspNetCore.Identity;

namespace StayEase.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CodeResetPassword { get; set; }
        public DateTime? CodeResetPasswordExpiration { get; set; }

        public string? RefresshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}

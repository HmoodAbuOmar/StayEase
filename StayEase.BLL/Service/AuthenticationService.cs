using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;

namespace StayEase.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration,
            IEmailSender emailSender, SignInManager<ApplicationUser> signInManager
            , ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user is null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid Email"
                    };
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Account is locked. Please try again later."
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                if (result.IsLockedOut)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Account is locked due to multiple failed login attempts. Please try again later."
                    };
                }

                else if (result.IsNotAllowed)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email is not confirmed. Please confirm your email before logging in."
                    };
                }

                if (!result.Succeeded)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid Password"
                    };
                }

                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GeneratedRefreshToken();
                user.RefresshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _userManager.UpdateAsync(user);


                return new LoginResponse
                {
                    Success = true,
                    Message = "Login Successfully",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "An Unexepted errors",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "User Creation Faield",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);

            var emailUrl = $"https://localhost:7142/api/Identity/Account/ConfirmEmail?token={token}&userid={user.Id}";
            await _emailSender.SendEmailAsync(user.Email, "Welcome to StayEase", $"<h1> Welcome .. {user.UserName}<h1> " +
                $"<a href='{emailUrl}'> Confirm Email <a/>");

            return new RegisterResponse
            {
                Success = true,
                Message = "Success"
            };
        }

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }

        public async Task<ForgetPasswordResponse> ForgetPassword(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ForgetPasswordResponse
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            var Random = new Random();
            var CodeResetPaswword = Random.Next(1000, 9999).ToString();
            user.CodeResetPassword = CodeResetPaswword;
            user.CodeResetPasswordExpiration = DateTime.UtcNow.AddMinutes(5);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "Password Reset Request",
                $"<h1>Password Reset Code</h1><p>Your password reset code is:" +
                $" <strong>{CodeResetPaswword}</strong></p>");

            return new ForgetPasswordResponse
            {
                Success = true,
                Message = "Password reset code sent to your email."
            };
        }
        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }

            if (user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Invalid Code."
                };
            }
            else if (user.CodeResetPasswordExpiration < DateTime.UtcNow)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Code has expired."
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Failed to reset password.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _emailSender.SendEmailAsync(request.Email, "Password Reset Successfuly",
                $"<p>your password is changed</p>");

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "Password reset successfully."

            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(TokenApiModel request)
        {
            string accessToken = request.AccessToken;
            string refreshToken = request.RefreshToken;
            var principal = _tokenService.GetPrincipalsFormExpiredToken(accessToken);

            var userName = principal.Identity.Name;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user is null || user.RefresshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {

                return new LoginResponse()
                {
                    Success = false,
                    Message = "invalid client request",
                };
            }

            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GeneratedRefreshToken();
            user.RefresshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Success = true,
                Message = "Token Refreshed",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };


        }

    }
}

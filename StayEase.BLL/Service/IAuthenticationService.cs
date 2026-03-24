using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<ForgetPasswordResponse> ForgetPassword(ForgetPasswordRequest request);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);

        Task<LoginResponse> RefreshTokenAsync(TokenApiModel request);

    }
}

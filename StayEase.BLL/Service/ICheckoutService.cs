using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId);
        Task<CheckoutResponse> HandleSuccessAsync(string sessionId);
    }
}

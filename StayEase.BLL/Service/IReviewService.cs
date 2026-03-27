using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface IReviewService
    {
        Task<BaseResponse> AddAsync(int hotelId, ReviewRequest request, string userId);
        Task<HotelReviewsResponse> GetByHotelAsync(int hotelId);
        Task<BaseResponse> UpdateAsync(int reviewId, ReviewRequest request, string userId);
        Task<BaseResponse> DeleteAsync(int reviewId, string userId);
    }
}

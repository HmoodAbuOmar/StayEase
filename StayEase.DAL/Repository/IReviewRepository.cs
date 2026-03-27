using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public interface IReviewRepository
    {
        Task<bool> HotelExistsAsync(int hotelId);
        Task<bool> UserCanReviewHotelAsync(int hotelId, string userId);
        Task<bool> HasUserReviewedHotelAsync(int hotelId, string userId);
        Task AddAsync(Review review);
        Task<List<Review>> GetByHotelAsync(int hotelId);
        Task<Review?> GetByIdAsync(int reviewId);
        Task DeleteAsync(Review review);

        Task UpdateAsync(Review review);
    }
}

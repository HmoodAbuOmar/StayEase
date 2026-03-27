using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<BaseResponse> AddAsync(int hotelId, ReviewRequest request, string userId)
        {
            if (!await _reviewRepository.HotelExistsAsync(hotelId))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Hotel not found"
                };
            }

            if (!await _reviewRepository.UserCanReviewHotelAsync(hotelId, userId))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You can only review a hotel you have reserved and paid for"
                };
            }

            if (await _reviewRepository.HasUserReviewedHotelAsync(hotelId, userId))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You already reviewed this hotel"
                };
            }

            var review = new Review
            {
                HotelId = hotelId,
                UserId = userId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);

            return new BaseResponse
            {
                Success = true,
                Message = "Review added successfully"
            };
        }

        public async Task<HotelReviewsResponse> GetByHotelAsync(int hotelId)
        {
            var reviews = await _reviewRepository.GetByHotelAsync(hotelId);

            return new HotelReviewsResponse
            {
                TotalReviews = reviews.Count,
                AverageRating = reviews.Count == 0
                    ? 0
                    : Math.Round((decimal)reviews.Average(r => r.Rating), 1),
                Reviews = reviews.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    UserName = r.User.UserName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };
        }

        public async Task<BaseResponse> UpdateAsync(int reviewId, ReviewRequest request, string userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);

            if (review is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Review not found"
                };
            }

            if (review.UserId != userId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You can only update your own review"
                };
            }
            review.Rating = request.Rating;
            review.Comment = request.Comment;

            await _reviewRepository.UpdateAsync(review);

            return new BaseResponse
            {
                Success = true,
                Message = "Review updated successfully"
            };
        }

        public async Task<BaseResponse> DeleteAsync(int reviewId, string userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);

            if (review is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Review not found"
                };
            }

            if (review.UserId != userId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You can only delete your own review"
                };
            }

            await _reviewRepository.DeleteAsync(review);

            return new BaseResponse
            {
                Success = true,
                Message = "Review deleted successfully"
            };
        }
    }
}

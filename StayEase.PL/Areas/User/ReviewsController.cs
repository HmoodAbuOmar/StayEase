using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayEase.BLL.Service;
using StayEase.DAL.DTO.Request;
using System.Security.Claims;

namespace StayEase.PL.Areas.User
{
    [Route("api/User/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("hotel/{hotelId}")]
        public async Task<IActionResult> AddReview(int hotelId, [FromBody] ReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var result = await _reviewService.AddAsync(hotelId, request, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _reviewService.UpdateAsync(reviewId, request, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _reviewService.DeleteAsync(reviewId, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }

}

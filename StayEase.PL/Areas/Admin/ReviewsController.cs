using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayEase.BLL.Service;

namespace StayEase.PL.Areas.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotel(int hotelId)
        {
            var result = await _reviewService.GetByHotelAsync(hotelId);
            return Ok(result);
        }
    }
}

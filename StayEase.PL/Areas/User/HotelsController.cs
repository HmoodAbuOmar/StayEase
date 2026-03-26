using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StayEase.BLL.Service;
using StayEase.PL.Resources;

namespace StayEase.PL.Areas.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]

    public class HotelsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IHotelService _hotelService;

        public HotelsController(IStringLocalizer<SharedResource> localizer, IHotelService hotelService)
        {
            _localizer = localizer;
            _hotelService = hotelService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetActiveHotel([FromQuery] string lang = "en")
        {
            var response = await _hotelService.GetActiveHotelAsync(lang);
            if (response is null)
            {
                return NotFound(new { message = "No active hotel found" });
            }
            return Ok(response);

        }
    }
}
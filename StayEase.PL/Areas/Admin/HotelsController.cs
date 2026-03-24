using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StayEase.BLL.Service;
using StayEase.DAL.DTO.Request;
using StayEase.PL.Resources;

namespace StayEase.PL.Areas.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> GetAllHotels([FromQuery] string lang = "en")
        {
            var response = await _hotelService.GetAllAsync(lang);

            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id, [FromQuery] string lang = "en")
        {
            var response = await _hotelService.GetByIdAsync(id, lang);

            if (response is null)
            {
                return NotFound(new { message = "Room type not found" });
            }

            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateHotel([FromBody] HotelRequest request)
        {
            var result = await _hotelService.Create(request);
            return Ok(new { message = _localizer["Success"].Value });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateHotels([FromRoute] int id, [FromBody] HotelRequest request)
        {
            var result = await _hotelService.UpdateHotelAsync(id, request);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int id)
        {
            var result = await _hotelService.DeleteAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}

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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public RoomsController(IRoomService roomService, IStringLocalizer<SharedResource> localizer)
        {
            _roomService = roomService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllRooms([FromQuery] string lang = "en")
        {
            var response = await _roomService.GetAllRoomsAsync(lang);

            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById([FromRoute] int id, [FromQuery] string lang = "en")
        {
            var response = await _roomService.FindRoomByIdAsync(id, lang);
            if (response is null)
            {
                return NotFound(new { message = "Room not found" });
            }
            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }

        [HttpGet("RoomNumber/{roomNumber}")]
        public async Task<IActionResult> GetByRoomNumber([FromRoute] string roomNumber, [FromQuery] string lang = "en")
        {
            var response = await _roomService.GetByRoomNumberAsync(roomNumber, lang);
            if (response is null)
            {
                return NotFound(new { message = "Room not found" });
            }
            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }


        [HttpPost("")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
        {
            var response = await _roomService.CreateRoomAsync(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomRequest request)
        {
            var result = await _roomService.UpdateRoomAsync(id, request);
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
        public async Task<IActionResult> DeleteRoom([FromRoute] int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
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

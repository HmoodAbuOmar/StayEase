using Microsoft.AspNetCore.Mvc;
using StayEase.BLL.Service;

namespace StayEase.PL.Areas.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAvaliableRoomsForUser([FromQuery] string lang = "en")
        {
            var result = await _roomService.GetAllAvaliableRoomAsync(lang);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}

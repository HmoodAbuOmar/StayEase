using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using StayEase.BLL.Service;
using StayEase.DAL.DTO.Request;
using StayEase.PL.Resources;

namespace StayEase.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public RoomTypesController(IRoomTypeService roomTypeService, IStringLocalizer<SharedResource> localizer)
        {
            _roomTypeService = roomTypeService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _roomTypeService.GetAllAsync();
            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }

        [HttpPost("")]

        public async Task<IActionResult> Create([FromBody] RoomTypeRequest request)
        {
            await _roomTypeService.CreateAsync(request);
            return Ok(new
            {
                message = _localizer["Success"].Value,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _roomTypeService.GetByIdAsync(id);
            if (response is null)
            {
                return NotFound(new { message = "Room type not found" });
            }
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RoomTypeRequest request)
        {
            var result = await _roomTypeService.UpdateAsync(id, request);
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _roomTypeService.DeleteAsync(id);
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

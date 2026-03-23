using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayEase.BLL.Service;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using System.Security.Claims;

namespace StayEase.PL.Areas.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reservationService.CreateReservationAsync(userId, request);

            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetMyReservation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = await _reservationService.GetMyReservationsAsync(userId);

            if (reservation is null)
            {
                return Ok(new
                {
                    message = "You do not have any reservations yet",
                    response = new List<AdminReservationResponse>()
                });
            }

            return Ok(reservation);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteReservationAsync([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reservationService.DeleteReservationAsync(userId, id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateReservationAsync([FromRoute] int id, [FromBody] ReservationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reservationService.UpdateReservationAsync(id, userId, request);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayEase.BLL.Service;
using StayEase.DAL.DTO.Response;

namespace StayEase.PL.Areas.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllReservations()
        {
            var response = await _reservationService.GetAllReservationsForAdminAsync();

            if (response == null || !response.Any())
            {
                return Ok(new
                {
                    message = "There are no reservations",
                    response = new List<AdminReservationResponse>()
                });
            }

            return Ok(new
            {
                message = "success",
                response
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllReservationsById([FromRoute] int id)
        {
            var response = await _reservationService.GetAllReservationsByIdForAdminAsync(id);

            if (response == null)
            {
                return BadRequest(new
                {
                    message = "There are no reservations for this id",
                });
            }

            return Ok(new
            {
                message = "success",
                response
            });
        }
        [HttpPatch("Confirm/{id}")]
        public async Task<IActionResult> ConfirmReservationForAdmin([FromRoute] int id)
        {
            var result = await _reservationService.ConfirmReservationForAdminAsync(id);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = "Reservation not found or cannot be confirmed"
                });
            }

            return Ok(new
            {
                message = "Reservation confirmed successfully"
            });
        }

        [HttpPatch("Cancel/{id}")]
        public async Task<IActionResult> CancelledReservationForAdmin([FromRoute] int id)
        {
            var result = await _reservationService.CancelReservationForAdminAsync(id);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = "Reservation not found or cannot be cancelled"
                });
            }

            return Ok(new
            {
                message = "Reservation cancelled successfully"
            });
        }

    }
}

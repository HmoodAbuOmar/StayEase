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
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var response = await _checkoutService.ProcessPaymentAsync(request, userId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var response = await _checkoutService.HandleSuccessAsync(session_id);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("cancel")]
        [AllowAnonymous]
        public IActionResult Cancel()
        {
            return Ok(new
            {
                Success = false,
                Message = "Payment was cancelled"
            });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using StayEase.BLL.Service;
using StayEase.DAL.DTO.Request;

namespace StayEase.PL.Areas.Identity
{
    [Route("api/Identity/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _authenticationService.RegisterAsync(registerRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await _authenticationService.LoginAsync(loginRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var result = await _authenticationService.ConfirmEmailAsync(token, userId);

            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            var result = await _authenticationService.ForgetPassword(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPassword(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

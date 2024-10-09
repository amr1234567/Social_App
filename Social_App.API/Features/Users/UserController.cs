using MediatR;
using Microsoft.AspNetCore.Mvc;
using Social_App.API.Features.Users.ChangePassword;
using Social_App.API.Features.Users.CheckEmailAvailable;
using Social_App.API.Features.Users.CheckUserNameAvailable;
using Social_App.API.Features.Users.Login;
using Social_App.API.Features.Users.RefreshToken;
using Social_App.API.Features.Users.RegisterUser;
using Social_App.API.Features.Users.RequestForResetPassword;
using Social_App.API.Features.Users.RevokeToken;
using Social_App.API.Features.Users.VerifyAccount;
using Social_App.API.Features.Users.VerifyChangingPasswordCodeToken;

namespace Social_App.API.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("verify-user")]
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyAccountRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("request-for-new-password")]
        public async Task<IActionResult> RequestForResetPassword([FromBody] RequestForResetPasswordRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("verify-new-password-request")]
        public async Task<IActionResult> VerifyChangingPasswordCodeToken([FromBody] VerifyChangingPasswordCodeTokenRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("check-user-name")]
        public async Task<IActionResult> CheckUserNameAvaliable([FromBody] CheckUserNameAvailableRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmailAvaliable([FromBody] CheckEmailAvailableRequest command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    
    }
}

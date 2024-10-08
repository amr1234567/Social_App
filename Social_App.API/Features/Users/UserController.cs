using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_App.API.Features.Users.RegisterUser;

namespace Social_App.API.Features.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}

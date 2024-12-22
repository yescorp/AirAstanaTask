using Application.Dtos.Incoming;
using Application.UseCases.Users.Commands.RegisterUser;
using Application.UseCases.Users.Queries.AuthorizeUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private IMediator _mediator;
        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto dto)
        {
            var command = new RegisterUserCommand(dto);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authorize(AuthorizeUserDto dto)
        {
            var query = new AuthorizeUserQuery(dto);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}

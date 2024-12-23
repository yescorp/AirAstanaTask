using Application.Common.Models;
using Application.Dtos.Incoming;
using Application.Dtos.Outgoing;
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

        /// <summary>
        /// Registers a user.
        /// The very first registered user is assigned a moderator role.
        /// Subsequent users are assigned a user role.
        /// Username must be unique.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponse>> RegisterUser(RegisterUserDto dto)
        {
            var command = new RegisterUserCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Login with username and password.
        /// Returns an access token that can be used to authorize requests that require authorization.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<AccessTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccessTokenResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponse<AccessTokenResponse>>> Authorize(AuthorizeUserDto dto)
        {
            var query = new AuthorizeUserQuery(dto);
            var result = await _mediator.Send(query);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}

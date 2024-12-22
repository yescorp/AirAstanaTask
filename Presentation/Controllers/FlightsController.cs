using Application.Dtos.Incoming;
using Application.UseCases.Flights.Commands.AddFlight;
using Application.UseCases.Flights.Commands.ChangeFlightStatus;
using Application.UseCases.Flights.Queries.GetFlights;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private IMediator _mediator;
        public FlightsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFlights([FromQuery] string? origin, [FromQuery] string? destination)
        {
            var query = new GetFlightsQuery(origin, destination);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Moderator)]
        public async Task<IActionResult> AddFlight([FromBody] AddFlightDto dto)
        {
            var command = new AddFlightCommand(dto);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("{flightId}")]
        [Authorize(Roles = RoleNames.Moderator)]
        public async Task<IActionResult> ChangeFlightStatus([FromRoute] int flightId, [FromBody] ChangeFlightStatusDto dto)
        {
            var command = new ChangeFlightStatusCommand(flightId, dto);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}

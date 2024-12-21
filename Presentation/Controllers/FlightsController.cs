using Application.Dtos.Incoming;
using Application.UseCases.Flights.Commands.AddFlight;
using Application.UseCases.Flights.Queries.GetFlights;
using MediatR;
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
        public async Task<IActionResult> GetFlights([FromQuery] string? origin, [FromQuery] string? destination)
        {
            var query = new GetFlightsQuery(origin, destination);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(AddFlightDto dto)
        {
            var command = new AddFlightCommand(dto);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}

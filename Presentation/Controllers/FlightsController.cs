using Application.Common.Models;
using Application.Dtos.Incoming;
using Application.Dtos.Outgoing;
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

        /// <summary>
        /// Get All Flights filtered by origin and/or destination, sorted by arrival.
        /// Either or both filters must be supplied.
        /// </summary>
        /// <param name="origin">Filters origin by equality</param>
        /// <param name="destination">Filters destination by equality</param>
        /// <returns>List of flights</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ActionResult<BaseResponse<IEnumerable<FlightResponse>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult<BaseResponse<IEnumerable<FlightResponse>>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse<IEnumerable<FlightResponse>>>> GetFlights([FromQuery] string? origin, [FromQuery] string? destination)
        {
            var query = new GetFlightsQuery(origin, destination);
            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Adds a new flight.
        /// Endpoint only for moderators.
        /// </summary>
        /// <param name="dto">Flight object</param>
        /// <remarks>
        /// The status must be one of the following:
        /// <list type="bullet">
        /// <item><c>InTime</c></item>
        /// <item><c>Delayed</c></item>
        /// <item><c>Cancelled</c></item>
        /// </list>
        /// The Arrival and Departure must be in one of the following formats (note that the time is saved with the provided offset):
        /// <list type="bullet">
        /// <item><c>2024-12-23T06:31:34.233Z</c>: as UTC time (0  offset)</item>
        /// <item><c>2024-12-23T06:31:34+02:00</c>: time with a positive offset from UTC</item>
        /// <item><c>2024-12-23T06:31:34-02:00</c>: time with a negative offset from UTC</item>
        /// </list>
        /// </remarks>
        /// <returns>Created flight object</returns>
        [HttpPost]
        [Authorize(Roles = RoleNames.Moderator)]
        [ProducesResponseType(typeof(ActionResult<BaseResponse<FlightResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult<BaseResponse<FlightResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResponse<FlightResponse>>> AddFlight([FromBody] AddFlightDto dto)
        {
            var command = new AddFlightCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Change status of the flight.
        /// Endpoint only for moderators.
        /// </summary>
        /// <param name="flightId">Id of the flight</param>
        /// <param name="dto">Flight Status, Either <c>InTime</c>, <c>Delayed</c>, <c>Cancelled</c></param>
        /// <returns></returns>
        [HttpPut("{flightId}")]
        [Authorize(Roles = RoleNames.Moderator)]
        [ProducesResponseType(typeof(ActionResult<BaseResponse<FlightResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult<BaseResponse<FlightResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<BaseResponse<FlightResponse>>> ChangeFlightStatus([FromRoute] int flightId, [FromBody] ChangeFlightStatusDto dto)
        {
            var command = new ChangeFlightStatusCommand(flightId, dto);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}

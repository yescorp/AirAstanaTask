using Application.Abstractions.Cqrs;
using Application.Common.Models;
using Application.Dtos.Outgoing;
using Application.UseCases.Flights.Queries.Filters;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Queries.GetFlights
{
    internal class GetFlightsQueryHandler : IQueryHandler<GetFlightsQuery, IEnumerable<FlightResponse>>
    {

        private IFlightsRepository _flightsRepository;

        public GetFlightsQueryHandler(IFlightsRepository flightsRepository)
        {
            _flightsRepository = flightsRepository;
        }

        public async Task<BaseResponse<IEnumerable<FlightResponse>>> Handle(GetFlightsQuery request, CancellationToken cancellationToken = default)
        {
            var filters = new IFilter<Flight>[] { new OriginFilter(request.Origin), new DestinationFilter(request.Destination) };
            var flights = await _flightsRepository.GetFilteredAsync(flight => flight.Arrival, filters, cancellationToken);
            var flightResponse = flights.Select(f => new FlightResponse(f));

            return new BaseResponse<IEnumerable<FlightResponse>>(true, flightResponse);
        }
    }
}

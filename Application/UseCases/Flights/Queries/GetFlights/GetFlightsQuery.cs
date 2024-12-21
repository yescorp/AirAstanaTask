using Application.Abstractions.Cqrs;
using Application.Dtos.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Queries.GetFlights
{
    public record GetFlightsQuery(string? Origin, string? Destination) : IQuery<IEnumerable<FlightResponse>>;
}

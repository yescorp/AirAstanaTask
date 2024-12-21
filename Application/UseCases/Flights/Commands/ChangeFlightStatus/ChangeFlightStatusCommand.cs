using Application.Abstractions.Cqrs;
using Application.Dtos.Incoming;
using Application.Dtos.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Commands.ChangeFlightStatus
{
    public record ChangeFlightStatusCommand(int Id, ChangeFlightStatusDto Dto) : ICommand<FlightResponse>;
}

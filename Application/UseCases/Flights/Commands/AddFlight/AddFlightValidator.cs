using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Commands.AddFlight
{
    public class AddFlightValidator : AbstractValidator<AddFlightCommand>
    {
        public AddFlightValidator()
        {
            RuleFor(x => x.Dto.Destination).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Dto.Origin).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Dto.Arrival).NotNull().NotEmpty();
            RuleFor(x => x.Dto.Departure).NotNull().NotEmpty();
            RuleFor(x => x.Dto.Arrival)
                .Must((command, arrival) =>
                {
                    return command.Dto.Departure.ToUniversalTime() < command.Dto.Arrival.ToUniversalTime();
                })
                .WithErrorCode("ArrivalValidationError")
                .WithMessage("The arrival time must be after the departure time");
        }
    }
}

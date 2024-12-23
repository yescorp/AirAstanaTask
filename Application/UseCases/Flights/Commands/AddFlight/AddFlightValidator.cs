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

            RuleFor(x => x.Dto).Must(dto =>
            {
                return dto.Origin != dto.Destination;
            })
                .WithErrorCode("FlightStructureError")
                .WithMessage("Origin and Destination can not be the same");

            RuleFor(x => x.Dto)
                .Must((dto) =>
                {
                    return dto.Departure.ToUniversalTime() < dto.Arrival.ToUniversalTime();
                })
                .WithErrorCode("ArrivalValidationError")
                .WithMessage("The arrival time must be after the departure time");
        }
    }
}

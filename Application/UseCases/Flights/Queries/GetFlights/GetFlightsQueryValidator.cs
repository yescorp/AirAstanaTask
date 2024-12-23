using Application.Common.Models;
using Application.Dtos.Outgoing;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Queries.GetFlights
{
    public class GetFlightsQueryValidator : AbstractValidator<GetFlightsQuery>
    {
        public GetFlightsQueryValidator()
        {
            RuleFor(x => x.Origin).MaximumLength(256);
            RuleFor(x => x.Destination).MaximumLength(256);

            RuleFor(x => x).Must(flight =>
            {
                return flight.Origin != null || flight.Destination != null;
            })
                .WithErrorCode("FilterError")
                .WithMessage("Either or both Origin and Destination filters should be specified");
        }
    }
}

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
            RuleFor(x => x.Origin).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Destination).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}

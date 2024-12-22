using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Queries.Filters
{
    public class DestinationFilter : IFilter<Flight>
    {
        public DestinationFilter(string? destination)
        {
            if (destination != null)
            {
                this.Filter = flight => flight.Destination == destination;
            }
            else
            {
                this.Filter = null;
            }
        }

        public Expression<Func<Flight, bool>>? Filter { get; init; }
    }
}

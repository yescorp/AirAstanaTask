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
    public class OriginFilter : IFilter<Flight>
    {
        public OriginFilter(string? origin)
        {
            if (origin != null)
            {
                Filter = f => f.Origin == origin;
            }
            else
            {
                Filter = null;
            }
        }

        public Expression<Func<Flight, bool>>? Filter { get; init; }
    }
}

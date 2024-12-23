using Application.UseCases.Flights.Queries.Filters;
using Domain.Entities;

namespace Domain.UnitTests
{
    public class FilterTests
    {
        [Fact]
        public void OriginFilter_WithNullOrigin_ReturnsNullExpression()
        {
            var origin = new OriginFilter(null);

            Assert.Null(origin.Filter);
        }

        [Fact]
        public void OriginFilter_WithNonNullOrigin_ReturnsFilterByOrigin()
        {
            var origin = "origin";
            var originFilter = new OriginFilter(origin);

            Assert.NotNull(originFilter.Filter);
        }

        [Fact]
        public void OriginFilter_ExpressionTest()
        {
            var origin = "origin";
            var originFilter = new OriginFilter(origin);

            var flights = new List<Flight>()
            {
                new Flight() { Origin = "other-origin" },
                new Flight() { Origin = origin },
                new Flight() { Origin = "some-other-origin" },
                new Flight() { Origin = origin }
            };

            var result = flights.Where(originFilter.Filter!.Compile()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Collection(result,
                flight =>
                {
                    Assert.Equal(flight.Origin, origin);
                },
                flight =>
                {
                    Assert.Equal(flight.Origin, origin);
                });
        }

        [Fact]
        public void DestinationFilter_WithNullOrigin_ReturnsNullExpression()
        {
            var destination = new DestinationFilter(null);

            Assert.Null(destination.Filter);
        }

        [Fact]
        public void DestinationFilter_WithNonNullOrigin_ReturnsFilterByDestination()
        {
            var destination = "destination";
            var destinationFilter = new DestinationFilter(destination);

            Assert.NotNull(destinationFilter.Filter);
        }

        [Fact]
        public void DestinationFilter_ExpressionTest()
        {
            var destination = "destination";
            var destinationFilter = new DestinationFilter(destination);

            var flights = new List<Flight>()
            {
                new Flight() { Destination = "other-destination" },
                new Flight() { Destination = destination },
                new Flight() { Destination = "some-other-destination" },
                new Flight() { Destination = destination }
            };

            var result = flights.Where(destinationFilter.Filter!.Compile()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Collection(result,
                flight =>
                {
                    Assert.Equal(flight.Destination, destination);
                },
                flight =>
                {
                    Assert.Equal(flight.Destination, destination);
                });
        }
    }
}
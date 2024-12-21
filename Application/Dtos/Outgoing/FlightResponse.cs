using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Outgoing
{
    public class FlightResponse
    {
        public FlightResponse(Flight flight)
        {
            Id = flight.Id;
            Origin = flight.Origin;
            Destination = flight.Destination;
            Departure = flight.Departure;
            Arrival = flight.Arrival;
            Status = flight.Status;
        }

        public int Id { get; set; }
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }
    }
}

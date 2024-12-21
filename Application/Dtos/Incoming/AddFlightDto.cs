using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Incoming
{
    public class AddFlightDto
    {
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }

        public Flight ToFlight()
        {
            return new Flight()
            {
                Origin = this.Origin,
                Destination = this.Destination,
                Departure = this.Departure,
                Arrival = this.Arrival,
                Status = this.Status
            };
        }
    }
}

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Flight : Entity
    {
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }
    }
}

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Incoming
{
    public class ChangeFlightStatusDto
    {
        public FlightStatus Status { get; set; }
    }
}

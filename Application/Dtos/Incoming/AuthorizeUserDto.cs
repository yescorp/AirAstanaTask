using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Incoming
{
    public class AuthorizeUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

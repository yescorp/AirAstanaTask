using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Incoming
{
    public class RegisterUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public User ToUser(string saltBase64)
        {
            return new User()
            {
                Username = this.Username!,
                Password = this.Password!,
                Salt = saltBase64
            };
        }
    }
}

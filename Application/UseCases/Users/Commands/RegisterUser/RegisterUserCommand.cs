using Application.Abstractions.Cqrs;
using Application.Dtos.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserDto Dto) : ICommand;
}

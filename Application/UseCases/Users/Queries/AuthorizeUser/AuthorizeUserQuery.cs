using Application.Abstractions.Cqrs;
using Application.Dtos.Incoming;
using Application.Dtos.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Queries.AuthorizeUser
{
    public record AuthorizeUserQuery(AuthorizeUserDto Dto) : IQuery<AccessTokenResponse>;
}

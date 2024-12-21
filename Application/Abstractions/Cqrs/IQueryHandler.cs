using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Cqrs
{
    public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, BaseResponse<TResult>>
    where TQuery : IQuery<TResult>
    {
    }
}

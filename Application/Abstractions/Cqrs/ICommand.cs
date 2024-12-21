using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Cqrs
{
    public interface ICommand : IRequest<BaseResponse>
    {
    }

    public interface ICommand<TResult> : IRequest<BaseResponse<TResult>>
    {
    }
}

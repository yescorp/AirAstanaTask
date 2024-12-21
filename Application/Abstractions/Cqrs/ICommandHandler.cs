using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Cqrs
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, BaseResponse>
    where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, BaseResponse<TResult>>
        where TCommand : ICommand<TResult>
    {
    }
}

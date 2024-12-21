using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IFilter<TEntity>
    {
        public Expression<Func<TEntity, bool>>? Filter { get; init; }
    }
}

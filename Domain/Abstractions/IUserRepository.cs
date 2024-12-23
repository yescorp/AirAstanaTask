﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User?> GetUserWithRole(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default);
    }
}

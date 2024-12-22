using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AirAstanaContext _dbContext;

        public UserRepository(AirAstanaContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserWithRole(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(filter, cancellationToken);
        }
    }
}

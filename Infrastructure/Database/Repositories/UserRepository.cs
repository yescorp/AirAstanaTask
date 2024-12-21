using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

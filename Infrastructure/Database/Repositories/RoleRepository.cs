using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AirAstanaContext _dbContext;

        public RoleRepository(AirAstanaContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

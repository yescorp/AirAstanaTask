using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
{

    public class FlightsRepository : GenericRepository<Flight>, IFlightsRepository
    {
        private readonly AirAstanaContext _dbContext;

        public FlightsRepository(AirAstanaContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

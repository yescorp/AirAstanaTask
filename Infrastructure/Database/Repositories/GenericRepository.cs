using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AirAstanaContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await _dbSet.AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<TEntity>> GetFilteredAsync<TOrderBy>(Expression<Func<TEntity, TOrderBy>>? orderBy, IFilter<TEntity>[] filters, CancellationToken cancellationToken = default)
        {
            var entities = _dbSet.AsQueryable();
            foreach (var filter in filters.Where(f => f.Filter != null).Select(f => f.Filter!))
            {
                entities = entities.Where(filter);
            }

            if (orderBy != null)
            {
                entities = entities.OrderBy(orderBy);
            }

            return await entities.ToListAsync(cancellationToken);
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = _dbSet.Update(entity);
            return Task.FromResult(result.Entity);
        }
    }
}

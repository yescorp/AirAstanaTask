using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IRepository<TEntity>
    where TEntity : class
    {
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        public Task<IEnumerable<TEntity>> GetFilteredAsync<TOrderBy>(Expression<Func<TEntity, TOrderBy>>? orderBy, IFilter<TEntity>[] filters, CancellationToken cancellationToken = default);
        
        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    }
}

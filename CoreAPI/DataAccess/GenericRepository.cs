using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreAPI.DataAccess
{
    public class GenericRepository<TEntity, TContext> : IRepository<TEntity> where TEntity : class where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private IQueryable<TEntity> _query;

        public GenericRepository(TContext context)
        {
            _context = context;
            _dbSet = _context?.Set<TEntity>();
            _query = _dbSet;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            try
            {
                if (includeProperties != null)
                {
                    _query = includeProperties(_query);
                }

                return await (predicate == null ? _query.ToListAsync() : _query.Where(predicate).ToListAsync()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null)
        {
            try
            {
                if (includeProperties != null)
                {
                    _query = includeProperties(_query);
                }

                return await _query.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

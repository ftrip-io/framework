using ftrip.io.framework.Domain;
using ftrip.io.framework.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Sql.Repository
{
    public class Repository<T, TId> : IRepository<T, TId> where TId : IEquatable<TId> where T : class, IIdentifiable<TId>
    {
        protected DbContext _context;
        protected DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> Read(CancellationToken cancellationToken = default)
        {
            return await _entities.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> Read(TId id, CancellationToken cancellationToken = default)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        public virtual IQueryBuilder<T> Query()
        {
            return new QueryBuilder<T>(_entities);
        }

        public virtual async Task<T> Create(T entity, CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);

            return entity;
        }

        public virtual async Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            var existingEntity = await Read(entity.Id, cancellationToken);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            return existingEntity;
        }

        public virtual async Task<T> Delete(TId id, CancellationToken cancellationToken = default)
        {
            var existingEntity = await Read(id, cancellationToken);
            if (existingEntity != null)
            {
                _entities.Remove(existingEntity);
            }

            return existingEntity;
        }

        public virtual Task<IEnumerable<T>> DeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _entities.RemoveRange(entities);

            return Task.FromResult(entities);
        }
    }
}
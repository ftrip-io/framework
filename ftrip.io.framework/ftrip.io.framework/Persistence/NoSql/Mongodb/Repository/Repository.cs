using ftrip.io.framework.Domain;
using ftrip.io.framework.Persistence.Contracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Repository
{
    public class Repository<T, TId> : IRepository<T, TId> where TId : IEquatable<TId> where T : class, IIdentifiable<TId>
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> Read(CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().ToListAsync(cancellationToken);
        }

        public async Task<T> Read(TId id, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryBuilder<T> Query()
        {
            return new QueryBuilder<T>(_collection.AsQueryable());
        }

        public async Task<T> Create(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);

            return entity;
        }

        public async Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            var existingEntity = await Read(entity.Id, cancellationToken);
            if (existingEntity != null)
            {
                await _collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, new ReplaceOptions(), cancellationToken);
            }

            return entity;
        }

        public async Task<T> Delete(TId id, CancellationToken cancellationToken = default)
        {
            var existingEntity = await Read(id, cancellationToken);
            if (existingEntity != null)
            {
                await _collection.DeleteOneAsync(e => e.Id.Equals(id), cancellationToken);
            }

            return existingEntity;
        }

        public async Task<IEnumerable<T>> DeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(e => entities.Any(i => e.Id.Equals(i.Id)), cancellationToken);

            return await Task.FromResult(entities);
        }
    }
}
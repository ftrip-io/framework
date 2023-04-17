using ftrip.io.framework.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Contracts
{
    public interface IRepository<T, TId> where TId : IEquatable<TId> where T : class, IIdentifiable<TId>
    {
        Task<IEnumerable<T>> Read(CancellationToken cancellationToken = default);

        Task<T> Read(TId id, CancellationToken cancellationToken = default);

        IQueryBuilder<T> Query();

        Task<T> Create(T entity, CancellationToken cancellationToken = default);

        Task<T> Update(T entity, CancellationToken cancellationToken = default);

        Task<T> Delete(TId id, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> DeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
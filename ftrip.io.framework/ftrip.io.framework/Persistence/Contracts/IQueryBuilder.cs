using ftrip.io.framework.Persistence.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Contracts
{
    public interface IQueryBuilder<T> where T : class
    {
        IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate);

        IQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector) where TResult : class;

        IQueryBuilder<T> Include(string navigationPropertyPath);

        IQueryBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);

        Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<bool> ContainsAsync(T item, CancellationToken cancellationToken = default);

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> FirstAsync(CancellationToken cancellationToken = default);

        Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> LastAsync(CancellationToken cancellationToken = default);

        Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> LastOrDefaultAsync(CancellationToken cancellationToken = default);

        Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<T>> ToListAsync(CancellationToken cancellationToken = default);

        Task<List<T>> PaginateAsync(Page page, CancellationToken cancellationToken = default);

        Task<PageResult<T>> ToPageResultAsync(Page page, CancellationToken cancellationToken = default);
    }
}
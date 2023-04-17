using MongoDB.Driver.Linq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Extensions
{
    public static class LastExtensions
    {
        public static async Task<TSource> LastAsync<TSource>(this IMongoQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            var numberOfEntities = await source.CountAsync(cancellationToken);

            if (numberOfEntities == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            return await source.Skip(numberOfEntities - 1).FirstAsync(cancellationToken);
        }

        public static async Task<TSource> LastAsync<TSource>(this IMongoQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var numberOfEntities = await source.CountAsync(predicate, cancellationToken);

            if (numberOfEntities == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            return await source.Where(predicate).Skip(numberOfEntities - 1).FirstAsync(cancellationToken);
        }

        public static async Task<TSource> LastOrDefaultAsync<TSource>(this IMongoQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            var numberOfEntities = await source.CountAsync(cancellationToken);

            if (numberOfEntities == 0)
            {
                return default;
            }

            return await source.Skip(numberOfEntities - 1).FirstOrDefaultAsync(cancellationToken);
        }

        public static async Task<TSource> LastOrDefaultAsync<TSource>(this IMongoQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var numberOfEntities = await source.CountAsync(predicate, cancellationToken);

            if (numberOfEntities == 0)
            {
                return default;
            }

            return await source.Where(predicate).Skip(numberOfEntities - 1).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
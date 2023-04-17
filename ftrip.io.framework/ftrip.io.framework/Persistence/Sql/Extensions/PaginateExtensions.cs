using ftrip.io.framework.Persistence.UtilityClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Sql.Extensions
{
    public static class PaginateExtensions
    {
        public static async Task<PageResult<TSource>> ToPageResult<TSource>(this DbSet<TSource> dbSet, Page page, CancellationToken cancellationToken = default) where TSource : class
        {
            return await dbSet.AsQueryable().ToPageResult(page, cancellationToken);
        }

        public static async Task<PageResult<TSource>> ToPageResult<TSource>(this IQueryable<TSource> source, Page page, CancellationToken cancellationToken = default)
        {
            var totalEntities = await source.CountAsync(cancellationToken);
            var entities = await source.Paginate(page, cancellationToken);

            return new PageResult<TSource>
            {
                Entities = entities,
                TotalEntities = totalEntities,
                TotalPages = (int)Math.Ceiling(totalEntities / (double)page.Size)
            };
        }

        public static async Task<IEnumerable<TSource>> Paginate<TSource>(this DbSet<TSource> dbSet, Page page, CancellationToken cancellationToken = default) where TSource : class
        {
            return await dbSet.AsQueryable().Paginate(page, cancellationToken);
        }

        public static async Task<IEnumerable<TSource>> Paginate<TSource>(this IQueryable<TSource> source, Page page, CancellationToken cancellationToken = default)
        {
            var numberOfEntitiesToSkip = (page.Number - 1) * page.Size;

            return await source.Skip(numberOfEntitiesToSkip).Take(page.Size).ToListAsync(cancellationToken);
        }
    }
}
﻿using ftrip.io.framework.Persistence.Contracts;
using ftrip.io.framework.Persistence.NoSql.Mongodb.Extensions;
using ftrip.io.framework.Persistence.UtilityClasses;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Repository
{
    public class QueryBuilder<T> : IQueryBuilder<T> where T : class
    {
        private IMongoQueryable<T> _query;

        public QueryBuilder(IMongoQueryable<T> query)
        {
            _query = query;
        }

        public IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);

            return this;
        }

        public IQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector) where TResult : class
        {
            return new QueryBuilder<TResult>(_query.Select(selector));
        }

        public IQueryBuilder<T> Include(string navigationPropertyPath)
        {
            throw new NotImplementedException();
        }

        public IQueryBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_query.All(predicate));
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _query.AnyAsync(predicate, cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await _query.AnyAsync(cancellationToken);
        }

        public Task<bool> ContainsAsync(T item, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_query.Contains(item));
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _query.CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _query.CountAsync(predicate, cancellationToken);
        }

        public async Task<T> FirstAsync(CancellationToken cancellationToken = default)
        {
            return await _query.FirstAsync(cancellationToken);
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _query.FirstAsync(predicate, cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T> LastAsync(CancellationToken cancellationToken = default)
        {
            return await _query.LastAsync(cancellationToken);
        }

        public async Task<T> LastAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _query.LastAsync(predicate, cancellationToken);
        }

        public async Task<T> LastOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await _query.LastOrDefaultAsync(cancellationToken);
        }

        public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _query.LastOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return await _query.ToListAsync();
        }

        public async Task<List<T>> PaginateAsync(Page page, CancellationToken cancellationToken = default)
        {
            return (await _query.Paginate(page, cancellationToken)).ToList();
        }

        public async Task<PageResult<T>> ToPageResultAsync(Page page, CancellationToken cancellationToken = default)
        {
            return await _query.ToPageResult(page, cancellationToken);
        }
    }
}
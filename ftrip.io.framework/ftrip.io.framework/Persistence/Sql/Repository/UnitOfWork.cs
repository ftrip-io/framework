using ftrip.io.framework.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Sql.Repository
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        protected DbContext _context;
        protected IDbContextTransaction _transaction;
        protected int _transactionCount;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _transactionCount = 0;
            _disposed = false;
        }

        public async Task Begin(CancellationToken cancellationToken = default)
        {
            _transactionCount++;

            if (_transactionCount > 1)
            {
                return;
            }

            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                return;
            }

            _transactionCount--;

            if (_transactionCount > 0)
            {
                return;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
                _transaction = null;
            }
            catch
            {
                await _transaction.RollbackAsync(cancellationToken);
                _transaction = null;
                throw;
            }
        }

        public async Task Rollback(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                return;
            }

            _transactionCount--;

            if (_transactionCount > 0)
            {
                return;
            }

            await _transaction.RollbackAsync(cancellationToken);
            _transaction = null;
        }

        #region Implements IDisposable

        #region Private Dispose Fields

        private bool _disposed;

        #endregion Private Dispose Fields

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <returns><see cref="ValueTask"/></returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);

            // Take this object off the finalization queue to prevent
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Whether or not we are disposing</param>
        /// <returns><see cref="ValueTask"/></returns>
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (!disposing)
            {
                return;
            }

            await _context.DisposeAsync();

            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
        }

        #endregion Implements IDisposable
    }
}
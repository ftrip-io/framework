using ftrip.io.framework.Persistence.Contracts;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.NoSql.Mongodb.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly IMongoClient _mongoClient;
        protected IClientSessionHandle _session;
        protected int _transactionCount;

        public UnitOfWork(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
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

            _session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            _session.StartTransaction();
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            if (_session == null)
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
                if (_session is { IsInTransaction: true })
                {
                    await _session.CommitTransactionAsync(cancellationToken);
                }
            }
            catch
            {
                if (_session is { IsInTransaction: true })
                {
                    await _session.AbortTransactionAsync(cancellationToken);
                }
                throw;
            }
            finally
            {
                if (_session != null)
                {
                    _session.Dispose();
                    _session = null;
                }
            }
        }

        public async Task Rollback(CancellationToken cancellationToken = default)
        {
            if (_session == null)
            {
                return;
            }

            _transactionCount--;

            if (_transactionCount > 0)
            {
                return;
            }

            if (_session is { IsInTransaction: true })
            {
                await _session.AbortTransactionAsync(cancellationToken);
            }
            _session?.Dispose();
            _session = null;
        }

        #region Implements IDisposable

        #region Private Dispose Fields

        private bool _disposed;

        #endregion Private Dispose Fields

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <returns><see cref="ValueTask"/></returns>
        public void Dispose()
        {
            Dispose(true);

            // Take this object off the finalization queue to prevent
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Whether or not we are disposing</param>
        /// <returns><see cref="ValueTask"/></returns>
        protected virtual void Dispose(bool disposing)
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

            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }
        }

        #endregion Implements IDisposable
    }
}
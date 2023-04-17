using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.Persistence.Contracts
{
    public interface IUnitOfWork
    {
        Task Begin(CancellationToken cancellationToken = default);

        Task Commit(CancellationToken cancellationToken = default);

        Task Rollback(CancellationToken cancellationToken = default);
    }
}
using System.Threading.Tasks;

namespace ftrip.io.framework.jobs.Contracts
{
    public interface IAsyncExecutable
    {
        Task Execute();
    }
}
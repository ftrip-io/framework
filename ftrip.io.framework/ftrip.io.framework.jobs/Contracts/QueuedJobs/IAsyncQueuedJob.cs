namespace ftrip.io.framework.jobs.Contracts.QueuedJobs
{
    public interface IAsyncQueuedJob : IAsyncExecutable
    {
        string Name { get; }
    }
}
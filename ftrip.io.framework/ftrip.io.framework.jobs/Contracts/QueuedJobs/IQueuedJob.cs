namespace ftrip.io.framework.jobs.Contracts.QueuedJobs
{
    public interface IQueuedJob : IExecutable
    {
        string Name { get; }
    }
}
namespace ftrip.io.framework.jobs.Contracts.ChronJobs
{
    public interface IAsyncCronJob : IAsyncExecutable
    {
        string Name { get; }
        string Cron { get; }
    }
}
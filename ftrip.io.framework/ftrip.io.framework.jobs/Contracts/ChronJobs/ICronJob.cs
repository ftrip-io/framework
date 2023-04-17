namespace ftrip.io.framework.jobs.Contracts.ChronJobs
{
    public interface ICronJob : IExecutable
    {
        string Name { get; }
        string Cron { get; }
    }
}
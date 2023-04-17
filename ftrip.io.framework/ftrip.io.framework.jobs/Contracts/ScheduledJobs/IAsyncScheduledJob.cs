using System;

namespace ftrip.io.framework.jobs.Contracts.ScheduledJobs
{
    public interface IAsyncScheduledJob : IAsyncExecutable
    {
        string Name { get; }
        TimeSpan In { get; }
    }
}
using System;

namespace ftrip.io.framework.jobs.Contracts.ScheduledJobs
{
    public interface IScheduledJob : IExecutable
    {
        string Name { get; }
        TimeSpan In { get; }
    }
}
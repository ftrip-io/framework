using ftrip.io.framework.jobs.Attributes;
using ftrip.io.framework.jobs.Contracts;
using ftrip.io.framework.jobs.Contracts.ChronJobs;
using ftrip.io.framework.jobs.Contracts.QueuedJobs;
using ftrip.io.framework.jobs.Contracts.ScheduledJobs;
using ftrip.io.framework.Utilities;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ftrip.io.framework.jobs.Extensions
{
    public static class JobManagerExtensions
    {
        public static IApplicationBuilder UseFtripionJobs<TAssembly>(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var recurringJobManager = serviceProvider.GetService(typeof(IRecurringJobManager)) as IRecurringJobManager;

            UseCronJobs<TAssembly>(serviceProvider, recurringJobManager);
            UseQueuedJobs<TAssembly>(serviceProvider);
            UseScheduledJobs<TAssembly>(serviceProvider);

            return app;
        }

        private static void UseCronJobs<TAssembly>(IServiceProvider serviceProvider, IRecurringJobManager recurringJobManager)
        {
            FindStartableJobs<TAssembly, ICronJob>(serviceProvider)
                .ForEach(cronJob => recurringJobManager.AddOrUpdate(cronJob.Name, () => cronJob.Execute(), cronJob.Cron));

            FindStartableJobs<TAssembly, IAsyncCronJob>(serviceProvider)
                .ForEach(cronJob => recurringJobManager.AddOrUpdate(cronJob.Name, () => cronJob.Execute(), cronJob.Cron));
        }

        private static void UseQueuedJobs<TAssembly>(IServiceProvider serviceProvider)
        {
            FindStartableJobs<TAssembly, IQueuedJob>(serviceProvider)
                .ForEach(queuedJob => BackgroundJob.Enqueue(() => queuedJob.Execute()));

            FindStartableJobs<TAssembly, IAsyncQueuedJob>(serviceProvider)
                .ForEach(queuedJob => BackgroundJob.Enqueue(() => queuedJob.Execute()));
        }

        private static void UseScheduledJobs<TAssembly>(IServiceProvider serviceProvider)
        {
            FindStartableJobs<TAssembly, IScheduledJob>(serviceProvider)
                .ForEach(scheduledJob => BackgroundJob.Schedule(() => scheduledJob.Execute(), scheduledJob.In));

            FindStartableJobs<TAssembly, IAsyncScheduledJob>(serviceProvider)
                .ForEach(scheduledJob => BackgroundJob.Schedule(() => scheduledJob.Execute(), scheduledJob.In));
        }

        private static List<TJob> FindStartableJobs<TAssembly, TJob>(IServiceProvider serviceProvider)
        {
            return typeof(TAssembly).Assembly
                .ExportedTypes
                .Where(type => typeof(TJob).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Where(ShouldRunOnStartup)
                .Select(type => Activator.CreateInstance(type, serviceProvider))
                .Cast<TJob>()
                .ToList();
        }

        private static bool ShouldRunOnStartup(Type type)
        {
            var attribute = GetRunOnStartupAttribute(type);
            if (attribute == null)
            {
                return false;
            }

            return attribute.Should;
        }

        private static RunOnStartupAttribute GetRunOnStartupAttribute(Type type) => CustomAttributeFinder.GetFrom<RunOnStartupAttribute>(type).FirstOrDefault();
    }
}
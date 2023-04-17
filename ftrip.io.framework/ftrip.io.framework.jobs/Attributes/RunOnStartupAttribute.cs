using System;

namespace ftrip.io.framework.jobs.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RunOnStartupAttribute : Attribute
    {
        public bool Should { get; set; } = true;

        public RunOnStartupAttribute()
        {
        }
    }
}
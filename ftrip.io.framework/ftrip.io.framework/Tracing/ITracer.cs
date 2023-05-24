using System.Diagnostics;

namespace ftrip.io.framework.Tracing
{
    public interface ITracer
    {
        ActivitySource ActivitySource { get; }
    }

    public class Tracer : ITracer
    {
        public ActivitySource ActivitySource { get; }

        public Tracer(string activitySourceName, string activityVersion)
        {
            ActivitySource = new ActivitySource(activitySourceName, activityVersion);
        }
    }
}
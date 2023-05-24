using OpenTelemetry;
using System.Diagnostics;
using System.Linq;

namespace ftrip.io.framework.Tracing
{
    internal sealed class SpamTracesProcessor : BaseProcessor<Activity>
    {
        private static readonly string[] _spamPaths = new[]
        {
            "/swagger/",
            "/api/health",
            "/healthcheck",
            "/ui/resources/"
        };

        public override void OnEnd(Activity activity)
        {
            if (IsSpamEndpoint(activity.DisplayName))
            {
                activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
            }
        }

        private static bool IsSpamEndpoint(string displayName)
        {
            return _spamPaths.Any(path => displayName.Contains(path));
        }
    }
}
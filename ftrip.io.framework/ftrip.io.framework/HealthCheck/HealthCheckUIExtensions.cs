using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ftrip.io.framework.HealthCheck
{
    public static class HealthCheckUIExtensions
    {
        public static IApplicationBuilder UseFtripioHealthCheckUI(this IApplicationBuilder app, HealthCheckUISettings settings)
        {
            app.UseHealthChecks("/api/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            })
            .UseHealthChecksUI(options =>
            {
                options.ApiPath = settings.ApiPath;
                options.UIPath = settings.UIPath;
            });

            return app;
        }
    }
}
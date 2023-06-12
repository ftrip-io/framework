using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace ftrip.io.framework.Metrics
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _next;

        public VisitorCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, VisitorCounter counter)
        {
            await _next(httpContext);
            if (httpContext.Request.Path.Value == "/metrics") return;

            string clientIpAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? httpContext.Request.Headers["X-Real-IP"].FirstOrDefault()
                ?? httpContext.Connection.RemoteIpAddress.ToString();
            counter.RegisterRequest(clientIpAddress, httpContext.Request.Headers["User-Agent"].ToString());
        }
    }

    public class DataTrafficCounterMiddleware
    {
        private readonly RequestDelegate _next;

        public DataTrafficCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, DataTrafficCounter counter)
        {
            if (httpContext.Request.Path.Value == "/metrics")
            {
                await _next(httpContext);
                return;
            }
            httpContext.Response.Body = new ContentLengthTrackingStream(httpContext.Response.Body);
            await _next(httpContext);
            counter.RegisterTraffic(httpContext.Response.Body.Length);
        }
    }

    public static class MetricsExtensions
    {
        public static void UseMetrics(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseMiddleware<VisitorCounterMiddleware>();
            app.UseMiddleware<DataTrafficCounterMiddleware>();
        }
    }
}

using Microsoft.AspNetCore.Builder;

namespace ftrip.io.framework.Correlation
{
    public static class CorrelationExtensions
    {
        public static void UseCorrelation(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationMiddleware>();
        }
    }
}
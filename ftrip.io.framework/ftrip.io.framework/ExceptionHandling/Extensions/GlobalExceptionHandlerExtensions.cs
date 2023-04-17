using ftrip.io.framework.ExceptionHandling.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ftrip.io.framework.ExceptionHandling.Extensions
{
    public static class GlobalExceptionHandlerExtensions
    {
        public static void UseFtripioGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
        }
    }
}
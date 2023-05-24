using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace ftrip.io.framework.Correlation
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, CorrelationContext correlationContext, ILogger logger)
        {
            var correlationId = GetCorrelationId(context);

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                logger.Information("Extracted data from HttpContext - CorrelationId[{CorrelationId}]");

                AddCorrelationIdToCorrelationContext(correlationContext, correlationId);
                AddCorrelationIdHeaderToResponse(context, correlationId);

                await _next(context);
            }
        }

        private static string GetCorrelationId(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(CorrelationConstants.HeaderAttriute, out var correlationId))
            {
                return correlationId;
            }

            return new StringValues(Guid.NewGuid().ToString());
        }

        private static void AddCorrelationIdToCorrelationContext(CorrelationContext correlationContext, StringValues correlationId)
        {
            correlationContext.Id = correlationId.ToString();
        }

        private static void AddCorrelationIdHeaderToResponse(HttpContext context, StringValues correlationId)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(CorrelationConstants.HeaderAttriute, correlationId);

                return Task.CompletedTask;
            });
        }
    }
}
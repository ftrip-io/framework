using ftrip.io.framework.Contexts;
using ftrip.io.framework.Correlation;
using GreenPipes;
using MassTransit;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Filters
{
    public class PopulateLogContextFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly CurrentUserContext _currentUserContext;
        private readonly CorrelationContext _correlationContext;

        public PopulateLogContextFilter(
            CurrentUserContext currentUserContext,
            CorrelationContext correlationContext)
        {
            _currentUserContext = currentUserContext;
            _correlationContext = correlationContext;
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            _currentUserContext.Id = "Consumer";
            _correlationContext.Id = context.CorrelationId?.ToString() ?? Guid.NewGuid().ToString();
            using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
            {
                await next.Send(context);
            }
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}
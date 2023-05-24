using ftrip.io.framework.Correlation;
using GreenPipes;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Filters
{
    public class PopulateCorrelationFilter<T> : IFilter<SendContext<T>> where T : class
    {
        private readonly CorrelationContext _correlationContext;

        public PopulateCorrelationFilter(CorrelationContext correlationContext)
        {
            _correlationContext = correlationContext;
        }

        public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            context.CorrelationId = Guid.Parse(_correlationContext.Id);

            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}
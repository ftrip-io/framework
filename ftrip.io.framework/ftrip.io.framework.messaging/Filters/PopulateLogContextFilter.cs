using GreenPipes;
using MassTransit;
using Serilog.Context;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Filters
{
    public class PopulateLogContextFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        public PopulateLogContextFilter()
        {
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
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
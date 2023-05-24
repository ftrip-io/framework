using ftrip.io.framework.Tracing;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.CQRS
{
    public class TracingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ITracer _tracer;

        public TracingBehaviour(ITracer tracer)
        {
            _tracer = tracer;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using var activity = _tracer.ActivitySource.StartActivity($"{request.GetType().Name} handle");

            return await next();
        }
    }
}
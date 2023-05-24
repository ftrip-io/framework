using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.CQRS
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.Information("Handling request - RequestName[{RequestName}]", request.GetType().Name);

            return await next();
        }
    }
}
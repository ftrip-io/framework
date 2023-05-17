﻿using MediatR;
using Serilog;
using System.Diagnostics;
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
            TResponse response;
            var requestName = request.GetType().Name;

            _logger.Information("Handling request - RequestName[{RequestName}]", requestName);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                _logger.Information("Finished handling request - RequestName[{RequestName}], ExecutionTime[{ExecutionTime}]", requestName, stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
using ftrip.io.framework.Correlation;
using ftrip.io.framework.Domain;
using ftrip.io.framework.messaging.Configurations;
using ftrip.io.framework.messaging.Filters;
using ftrip.io.framework.messaging.Settings;
using GreenPipes;
using MassTransit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IBus _bus;
        private readonly ILogger _logger;
        private readonly CorrelationContext _correlationContext;
        private readonly QueuesForEvent _queuesForEvent;
        private readonly RabbitMQSettings _settings;

        public MessagePublisher(
            IBus bus,
            ILogger logger,
            CorrelationContext correlationContext,
            QueuesForEvent queuesForEvent,
            RabbitMQSettings settings)
        {
            _bus = bus;
            _logger = logger;
            _correlationContext = correlationContext;
            _queuesForEvent = queuesForEvent;
            _settings = settings;
        }

        public async Task Send<T, TId>(T message, CancellationToken cancellationToken = default) where T : class, IEvent<TId> where TId : IEquatable<TId>
        {
            var queues = _queuesForEvent[typeof(T)];
            if (queues.Any())
            {
                await PublishToQueues<T, TId>(queues, message, cancellationToken);
                return;
            }

            await PublishToAll<T, TId>(message, cancellationToken);
        }

        private async Task PublishToAll<T, TId>(T message, CancellationToken cancellationToken = default) where T : class, IEvent<TId> where TId : IEquatable<TId>
        {
            var endPoint = await _bus.GetPublishSendEndpoint<T>();
            await endPoint.Send(message, GetPopulateCorrelationPipe<T>(), cancellationToken);

            _logger.Information("Published message to all consumers - Queue[{Queue}]", typeof(T).Name);
        }

        private async Task PublishToQueues<T, TId>(List<string> queues, T message, CancellationToken cancellationToken = default) where T : class, IEvent<TId> where TId : IEquatable<TId>
        {
            foreach (var queue in queues)
            {
                var uri = new Uri($"{_settings.GetConnectionString()}/{queue}");

                var endPoint = await _bus.GetSendEndpoint(uri);

                await endPoint.Send(message, GetPopulateCorrelationPipe<T>(), cancellationToken);
            }

            _logger.Information("Published message - Queues[{Queues}]", queues);
        }

        private IPipe<SendContext<T>> GetPopulateCorrelationPipe<T>() where T : class
        {
            return Pipe.New<SendContext<T>>(cfg =>
            {
                cfg.UseFilter(new PopulateCorrelationFilter<T>(_correlationContext));
            });
        }
    }
}
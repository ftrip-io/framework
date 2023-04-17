using ftrip.io.framework.Domain;
using ftrip.io.framework.messaging.Configurations;
using ftrip.io.framework.messaging.Settings;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IBus _bus;
        private readonly QueuesForEvent _queuesForEvent;
        private readonly RabbitMQSettings _settings;

        public MessagePublisher(IBus bus, QueuesForEvent queuesForEvent, RabbitMQSettings settings)
        {
            _bus = bus;
            _queuesForEvent = queuesForEvent;
            _settings = settings;
        }

        public async Task Send<T, TId>(T message, CancellationToken cancellationToken = default) where T : IEvent<TId> where TId : IEquatable<TId>
        {
            var queues = _queuesForEvent[typeof(T)];
            foreach (var queue in queues)
            {
                var uri = new Uri($"{_settings.GetConnectionString()}/{queue}");

                var endPoint = await _bus.GetSendEndpoint(uri);

                await endPoint.Send(message, cancellationToken);
            }
        }
    }
}
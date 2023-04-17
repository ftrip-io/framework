using ftrip.io.framework.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ftrip.io.framework.messaging.Publisher
{
    public interface IMessagePublisher
    {
        Task Send<T, TId>(T message, CancellationToken cancellationToken = default) where T : IEvent<TId> where TId : IEquatable<TId>;
    }
}
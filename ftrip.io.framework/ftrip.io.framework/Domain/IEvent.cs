using System;

namespace ftrip.io.framework.Domain
{
    public interface IEvent<TId> : IEvent, IIdentifiable<TId> where TId : IEquatable<TId>
    {
    }

    public interface IEvent
    {
        public string Type { get; }
        public DateTime OccuredAt { get; }
    }
}
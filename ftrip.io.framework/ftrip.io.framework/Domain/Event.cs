using System;

namespace ftrip.io.framework.Domain
{
    public class Event<TId> : IEvent<TId> where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
        public string Type { get => GetType().Name; }
        public DateTime OccuredAt { get; } = DateTime.UtcNow;
    }
}
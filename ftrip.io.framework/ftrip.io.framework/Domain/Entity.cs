using System;

namespace ftrip.io.framework.Domain
{
    public class Entity<TId> : IEntity<TId> where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
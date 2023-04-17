using System;

namespace ftrip.io.framework.Domain
{
    public interface IIdentifiable<TId> where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
    }
}
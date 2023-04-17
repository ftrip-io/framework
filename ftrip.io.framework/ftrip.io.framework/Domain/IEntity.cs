using System;

namespace ftrip.io.framework.Domain
{
    public interface IEntity<TId> : IIdentifiable<TId>, ISoftDeleteable, IAuditable where TId : IEquatable<TId>
    {
    }
}
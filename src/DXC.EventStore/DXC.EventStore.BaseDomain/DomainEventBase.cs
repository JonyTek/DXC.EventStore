using System;

namespace DXC.EventStore.BaseDomain
{
    public abstract class DomainEventBase<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public Guid EventId { get; protected set; }
        public TAggregateId AggregateId { get; protected set; }
        public long AggregateVersion { get; protected set; }

        protected DomainEventBase()
        {
            EventId = Guid.NewGuid();
        }

        public DomainEventBase<TAggregateId> WithVersion(long version)
        {
            AggregateVersion = version;
            return this;
        }
    }
}
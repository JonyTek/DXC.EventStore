using System.Collections.Generic;
using System.Linq;

namespace DXC.EventStore.BaseDomain
{
    public abstract class Aggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public long Version { get; protected set; } = -1;
        public TAggregateId Id { get; protected set; }

        private readonly ICollection<DomainEventBase<TAggregateId>> uncommittedEvents =
            new List<DomainEventBase<TAggregateId>>();

        public void ApplyEvent(DomainEventBase<TAggregateId> evnt, long version)
        {
            ((dynamic) this).Apply((dynamic) evnt.WithVersion(version));
            Version = version;
        }

        protected void RaiseEvent<TEvent>(TEvent evnt)
            where TEvent : DomainEventBase<TAggregateId>
        {
            ApplyEvent(evnt, Version + 1);
            uncommittedEvents.Add(evnt);
        }

        public void ClearUncommittedEvents()
        {
            uncommittedEvents.Clear();
        }

        public IEnumerable<DomainEventBase<TAggregateId>> GetUncommittedEvents()
        {
            return uncommittedEvents.AsEnumerable();
        }
    }
}

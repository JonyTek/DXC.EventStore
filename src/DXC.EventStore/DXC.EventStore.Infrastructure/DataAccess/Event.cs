using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Infrastructure.DataAccess
{
    public class Event<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public Event(DomainEventBase<TAggregateId> domainEvent, long eventNumber)
        {
            DomainEvent = domainEvent;
            EventNumber = eventNumber;
        }

        public long EventNumber { get; }

        public DomainEventBase<TAggregateId> DomainEvent { get; }
    }
}
using System.Threading.Tasks;

namespace DXC.EventStore.BaseDomain
{
    public interface IHandleDomainEvent<in TDomainEvent, in TAggregateId>
        where TDomainEvent : DomainEventBase<TAggregateId>
        where TAggregateId : IAggregateId
    {
        Task Handle(TDomainEvent evnt);
    }
}
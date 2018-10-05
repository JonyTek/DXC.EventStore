using System.Collections.Generic;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Infrastructure.DataAccess
{
    public interface IEventStore
    {
        Task AppendEvent<TAggregateId>(DomainEventBase<TAggregateId> evnt)
            where TAggregateId : IAggregateId;

        Task<IEnumerable<Event<TAggregateId>>> ReadEventsAsync<TAggregateId>(TAggregateId id)
            where TAggregateId : IAggregateId;
    }
}
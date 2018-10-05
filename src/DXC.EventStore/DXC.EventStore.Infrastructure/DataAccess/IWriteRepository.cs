using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Infrastructure
{
    public interface IWriteRepository<TAggregate, TAggregateId>
        where TAggregate : Aggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        Task SaveAsync(TAggregate aggregate);
        Task<TAggregate> GetByIdAsync(TAggregateId id);
    }
}
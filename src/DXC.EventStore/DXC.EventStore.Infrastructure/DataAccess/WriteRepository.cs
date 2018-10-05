using System;
using System.Reflection;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Infrastructure.DataAccess
{
    public class WriteRepository<TAggregate, TAggregateId> : IWriteRepository<TAggregate, TAggregateId>
        where TAggregate : Aggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        private readonly IEventStore eventStore;
        private readonly IDomainEventPublisher domainEventPublisher;

        public WriteRepository(IEventStore eventStore, IDomainEventPublisher domainEventPublisher)
        {
            this.eventStore = eventStore;
            this.domainEventPublisher = domainEventPublisher;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            foreach (var evnt in aggregate.GetUncommittedEvents())
            {
                await eventStore.AppendEvent(evnt);
                await domainEventPublisher.Publish((dynamic) evnt);
            }

            aggregate.ClearUncommittedEvents();
        }

        public async Task<TAggregate> GetByIdAsync(TAggregateId id)
        {
            var aggregate = CreateEmptyAggregate();

            foreach (var evnt in await eventStore.ReadEventsAsync(id))
            {
                aggregate.ApplyEvent(evnt.DomainEvent, evnt.EventNumber);
            }

            return aggregate;
        }

        private static TAggregate CreateEmptyAggregate()
        {
            var aggregate =  (TAggregate)typeof(TAggregate)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[0], new ParameterModifier[0])
                ?.Invoke(new object[0]);

            if (aggregate == null)
            {
                throw new ReflectionTypeLoadException(new[] {typeof(TAggregate)}, new Exception[0]);
            }

            return aggregate;
        }
    }
}
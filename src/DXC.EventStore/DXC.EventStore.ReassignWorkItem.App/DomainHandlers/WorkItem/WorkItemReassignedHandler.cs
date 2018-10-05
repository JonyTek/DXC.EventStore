using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.Domain.WorkItem;
using DXC.EventStore.Infrastructure.DataAccess;
using DXC.EventStore.ReadModel.WorkItem;

namespace DXC.EventStore.ReassignWorkItem.App.DomainHandlers.WorkItem
{
    public class WorkItemReassignedHandler : IHandleDomainEvent<WorkItemReassignedEvent, WorkItemId>
    {
        private readonly IReadRepository<WorkItemReadModel> repository;

        public WorkItemReassignedHandler(IReadRepository<WorkItemReadModel> repository)
        {
            this.repository = repository;
        }

        public async Task Handle(WorkItemReassignedEvent evnt)
        {
            var workItem = await repository.Find(evnt.AggregateId);
            workItem.BrokerId = evnt.BrokerId;
        }
    }
}
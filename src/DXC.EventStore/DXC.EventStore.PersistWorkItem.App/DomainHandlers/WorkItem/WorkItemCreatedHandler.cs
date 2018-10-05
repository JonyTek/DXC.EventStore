using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.Domain.WorkItem;
using DXC.EventStore.Infrastructure.DataAccess;
using DXC.EventStore.ReadModel.WorkItem;

namespace DXC.EventStore.PersistWorkItem.App.DomainHandlers.WorkItem
{
    public class WorkItemCreatedHandler : IHandleDomainEvent<WorkItemCreatedEvent, WorkItemId>
    {
        private readonly IReadRepository<WorkItemReadModel> repository;

        public WorkItemCreatedHandler(IReadRepository<WorkItemReadModel> repository)
        {
            this.repository = repository;
        }

        public async Task Handle(WorkItemCreatedEvent evnt)
        {
            var workItem = new WorkItemReadModel
            {
                Id = evnt.AggregateId.Value,
                ContractId = evnt.ContractId,
                BrokerId = evnt.BrokerId
            };

            await repository.Insert(workItem);
        }
    }
}
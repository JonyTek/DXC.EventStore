using System.Collections.Generic;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.Domain.WorkItem;
using DXC.EventStore.Infrastructure;
using DXC.EventStore.Infrastructure.DataAccess;
using WorkItemCreated = DXC.EventStore.Contracts.WorkItemCreated;

namespace DXC.EventStore.PersistWorkItem.App.Application
{
    public class CreateWorkItem : ICreateWorkItem
    {
        private readonly IEnumerable<IHandleDomainEvent<WorkItemCreatedEvent, WorkItemId>> workItemCreatedHandlers;
        private readonly IDomainEventSubscriber eventSubscriber;
        private readonly IWriteRepository<WorkItem, WorkItemId> writeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBus bus;

        public CreateWorkItem(IEnumerable<IHandleDomainEvent<WorkItemCreatedEvent, WorkItemId>> workItemCreatedHandlers,
            IDomainEventSubscriber eventSubscriber, IWriteRepository<WorkItem, WorkItemId> writeRepository,
            IUnitOfWork unitOfWork, IBus bus)
        {
            this.workItemCreatedHandlers = workItemCreatedHandlers;
            this.eventSubscriber = eventSubscriber;
            this.writeRepository = writeRepository;
            this.unitOfWork = unitOfWork;
            this.bus = bus;
        }

        public async Task Execute(Contracts.CreateWorkItem task)
        {
            using (unitOfWork)
            {
                var workItem = WorkItem.Create(task.ContractId, task.BrokerId);

                eventSubscriber.Subscribe<WorkItemCreatedEvent>(async x => await HandleAsync(workItemCreatedHandlers, x));

                await writeRepository.SaveAsync(workItem);

                await bus.Publish(new WorkItemCreated {WorkItemId = workItem.Id.Value, BrokerId = task.BrokerId});
            }
        }

        public async Task HandleAsync(
            IEnumerable<IHandleDomainEvent<WorkItemCreatedEvent, WorkItemId>> handlers, WorkItemCreatedEvent evnt)
        {
            foreach (var handler in handlers)
            {
                await handler.Handle(evnt);
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.Domain.WorkItem;
using DXC.EventStore.Infrastructure;
using DXC.EventStore.Infrastructure.DataAccess;

namespace DXC.EventStore.ReassignWorkItem.App.Application
{
    public interface IReassignWorkItem
    {
        Task Execute(Contracts.ReassignWorkItem task);
    }

    public class ReassignWorkItem : IReassignWorkItem
    {
        private readonly IEnumerable<IHandleDomainEvent<WorkItemReassignedEvent, WorkItemId>>
            workItemReassignedHandlers;
        private readonly IDomainEventSubscriber eventSubscriber;
        private readonly IWriteRepository<WorkItem, WorkItemId> writeRepository;
        private readonly IUnitOfWork unitOfWork;

        public ReassignWorkItem(
            IEnumerable<IHandleDomainEvent<WorkItemReassignedEvent, WorkItemId>> workItemReassignedHandlers,
            IWriteRepository<WorkItem, WorkItemId> writeRepository,
            IDomainEventSubscriber eventSubscriber, IUnitOfWork unitOfWork)
        {
            this.workItemReassignedHandlers = workItemReassignedHandlers;
            this.writeRepository = writeRepository;
            this.eventSubscriber = eventSubscriber;
            this.unitOfWork = unitOfWork;
        }

        public async Task Execute(Contracts.ReassignWorkItem task)
        {
            using (unitOfWork)
            {
                var workItem = await writeRepository.GetByIdAsync(WorkItemId.FromGuid(task.WorkItemId));

                workItem.Reassign(task.BrokerId);

                eventSubscriber.Subscribe<WorkItemReassignedEvent>(async x =>
                    await HandleAsync(workItemReassignedHandlers, x));

                await writeRepository.SaveAsync(workItem);
            }
        }

        public async Task HandleAsync(
            IEnumerable<IHandleDomainEvent<WorkItemReassignedEvent, WorkItemId>> handlers, WorkItemReassignedEvent evnt)
        {
            foreach (var handler in handlers)
            {
                await handler.Handle(evnt);
            }
        }
    }
}
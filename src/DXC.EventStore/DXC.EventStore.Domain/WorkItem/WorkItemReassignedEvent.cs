using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Domain.WorkItem
{
    public class WorkItemReassignedEvent : DomainEventBase<WorkItemId>
    {
        public string BrokerId { get; }

        public WorkItemReassignedEvent(WorkItemId aggregateId, string brokerId,
            long workItemVersion)
        {
            AggregateId = aggregateId;
            BrokerId = brokerId;
            AggregateVersion = workItemVersion;
        }
    }
}
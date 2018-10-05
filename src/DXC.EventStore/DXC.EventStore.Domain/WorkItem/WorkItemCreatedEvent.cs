using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Domain.WorkItem
{
    public class WorkItemCreatedEvent : DomainEventBase<WorkItemId>
    {
        public string ContractId { get; }
        public string BrokerId { get; }

        public WorkItemCreatedEvent(WorkItemId aggregateId, string contractId, string brokerId,
            long workItemVersion)
        {
            AggregateId = aggregateId;
            ContractId = contractId;
            BrokerId = brokerId;
            AggregateVersion = workItemVersion;
        }
    }
}
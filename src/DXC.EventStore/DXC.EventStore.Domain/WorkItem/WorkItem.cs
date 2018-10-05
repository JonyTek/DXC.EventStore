using System;
using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Domain.WorkItem
{
    public class WorkItem : Aggregate<WorkItemId>
    {
        public string ContractId { get; private set; }
        public string BrokerId { get; private set; }

        private WorkItem()
        {
            /*Required for reflection*/
        }

        private WorkItem(WorkItemId id, string contractId, string brokerId)
        {
            ContractId = contractId;
            Id = id;
            BrokerId = brokerId;
        }

        public static WorkItem Create(string contractId, string brokerId)
        {
            if (string.IsNullOrWhiteSpace(contractId))
            {
                throw new ArgumentException(nameof(contractId));
            }

            if (contractId.Length != 5)
            {
                throw new ArgumentException(nameof(contractId));
            }

            if (string.IsNullOrWhiteSpace(brokerId))
            {
                throw new ArgumentException(nameof(brokerId));
            }

            var workItem = new WorkItem(WorkItemId.New(), contractId, brokerId);

            workItem.RaiseEvent(new WorkItemCreatedEvent(workItem.Id, workItem.ContractId, workItem.BrokerId,
                workItem.Version));

            return workItem;
        }

        public void Reassign(string brokerId)
        {
            if (string.IsNullOrWhiteSpace(brokerId))
            {
                throw new ArgumentException(nameof(brokerId));
            }

            RaiseEvent(new WorkItemReassignedEvent(Id, brokerId, Version));
        }

        public void Apply(WorkItemCreatedEvent evnt)
        {
            Id = evnt.AggregateId;
            ContractId = evnt.ContractId;
            BrokerId = evnt.BrokerId;
            Version = evnt.AggregateVersion;
        }

        public void Apply(WorkItemReassignedEvent evnt)
        {
            BrokerId = evnt.BrokerId;
        }
    }
}
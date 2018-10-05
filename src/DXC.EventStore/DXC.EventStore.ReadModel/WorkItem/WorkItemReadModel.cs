using System;

namespace DXC.EventStore.ReadModel.WorkItem
{
    public class WorkItemReadModel : IReadModel
    {
        public Guid Id { get; set; }
        public string ContractId { get; set; }
        public string BrokerId { get; set; }
    }
}
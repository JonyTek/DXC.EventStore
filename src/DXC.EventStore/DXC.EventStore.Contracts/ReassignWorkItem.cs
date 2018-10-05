using System;

namespace DXC.EventStore.Contracts
{
    public class ReassignWorkItem
    {
        public Guid WorkItemId { get; set; }
        public string BrokerId { get; set; }
    }
}
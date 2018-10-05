using System;

namespace DXC.EventStore.Contracts
{
    public class WorkItemCreated : AbstractMessage
    {
        public override Uri Destination => new Uri("http://localhost:32088/api/workitem/publish");
        public Guid WorkItemId { get; set; }
        public string BrokerId { get; set; }
    }
}
using System;

namespace DXC.EventStore.Contracts
{
    public class CreateWorkItem : AbstractMessage
    {
        public override Uri Destination => new Uri("http://localhost:32087/api/workitem/persist");
        public string ContractId { get; set; }
        public string BrokerId { get; set; }
    }
}

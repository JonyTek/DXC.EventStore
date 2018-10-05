using System;
using DXC.EventStore.BaseDomain;

namespace DXC.EventStore.Domain.WorkItem
{
    public class WorkItemId : IAggregateId
    {
        public Guid Value { get; private set; }

        private WorkItemId() : this(Guid.NewGuid())
        {
        }

        private WorkItemId(Guid id) => Value = id;

        public static WorkItemId New() => new WorkItemId();

        public static WorkItemId FromGuid(Guid guid) => new WorkItemId(guid);

        public string AsString => $"{GetType().Name.ToLower()}-{Value}";
    }
}
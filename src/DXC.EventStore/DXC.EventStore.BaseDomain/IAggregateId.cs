using System;

namespace DXC.EventStore.BaseDomain
{
    public interface IAggregateId
    {
        Guid Value { get; }
        string AsString { get; }
    }
}
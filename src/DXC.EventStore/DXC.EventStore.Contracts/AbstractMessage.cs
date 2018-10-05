using System;

namespace DXC.EventStore.Contracts
{
    public abstract class AbstractMessage
    {
        public abstract Uri Destination { get; }
    }
}

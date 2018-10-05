using System;

namespace DXC.EventStore.BaseDomain
{
    public interface IDomainEventSubscriber
    {
        void Subscribe<T>(Action<T> handler);
    }
}
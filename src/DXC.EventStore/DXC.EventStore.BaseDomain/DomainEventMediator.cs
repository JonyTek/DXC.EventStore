using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DXC.EventStore.BaseDomain
{
    public class DomainEventMediator : IDomainEventSubscriber, IDomainEventPublisher
    {
        private static readonly AsyncLocal<Dictionary<Type, List<object>>> handlerMap =
            new AsyncLocal<Dictionary<Type, List<object>>>();

        public Dictionary<Type, List<object>> Handlers =>
            handlerMap.Value ?? (handlerMap.Value = new Dictionary<Type, List<object>>());

        public void Subscribe<T>(Action<T> handler)
        {
            GetHandlersOfType<T>().Add(handler);
        }

        public void Subscribe<T>(Func<T, Task> handler)
        {
            GetHandlersOfType<T>().Add(handler);
        }

        public async Task Publish<T>(T evnt)
        {
            var handlers = GetHandlersOfType<T>();

            foreach (var handler in handlers)
            {
                switch (handler)
                {
                    case Action<T> action:
                        action(evnt);
                        break;
                    case Func<T, Task> action:
                        await action(evnt);
                        break;
                }
            }
        }

        private ICollection<object> GetHandlersOfType<T>()
        {
            return Handlers.ContainsKey(typeof(T)) ? Handlers[typeof(T)] : (Handlers[typeof(T)] = new List<object>());
        }
    }
}
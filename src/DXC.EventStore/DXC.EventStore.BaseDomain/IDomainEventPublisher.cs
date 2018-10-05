using System.Threading.Tasks;

namespace DXC.EventStore.BaseDomain
{
    public interface IDomainEventPublisher
    {
        Task Publish<T>(T evnt);
    }
}
using System.Threading.Tasks;
using DXC.EventStore.Contracts;

namespace DXC.EventStore.Infrastructure
{
    public interface IBus
    {
        Task Publish<TMessage>(TMessage message) where TMessage : AbstractMessage;
    }
}
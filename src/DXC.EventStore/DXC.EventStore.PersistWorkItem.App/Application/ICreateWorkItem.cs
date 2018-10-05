using System.Threading.Tasks;

namespace DXC.EventStore.PersistWorkItem.App.Application
{
    public interface ICreateWorkItem
    {
        Task Execute(Contracts.CreateWorkItem task);
    }
}
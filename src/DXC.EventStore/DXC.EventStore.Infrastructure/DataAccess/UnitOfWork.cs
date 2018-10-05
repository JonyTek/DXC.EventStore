using Microsoft.EntityFrameworkCore;

namespace DXC.EventStore.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context.SaveChanges();
        }
    }
}
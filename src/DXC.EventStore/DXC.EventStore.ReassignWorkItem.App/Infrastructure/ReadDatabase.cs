using DXC.EventStore.ReadModel.WorkItem;
using Microsoft.EntityFrameworkCore;

namespace DXC.EventStore.ReassignWorkItem.App.Infrastructure
{
    //TODO: Find solution to preven this being duplicated across services
    public class ReadDatabase : DbContext
    {
        public DbSet<WorkItemReadModel> WorkItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=WorkItems;Trusted_Connection=True;");
        }
    }
}
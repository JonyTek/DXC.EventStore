using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.Infrastructure.DataAccess;
using DXC.EventStore.ReadModel.WorkItem;
using Microsoft.EntityFrameworkCore;

namespace DXC.EventStore.ReassignWorkItem.App.Infrastructure
{
    //TODO: Find solution to preven this being duplicated across services
    public class WorkItemReadModelRepository : IReadRepository<WorkItemReadModel>
    {
        private readonly ReadDatabase readDatabase;

        public WorkItemReadModelRepository(ReadDatabase readDatabase)
        {
            this.readDatabase = readDatabase;
        }

        public async Task Insert(WorkItemReadModel model)
        {
            await readDatabase.WorkItems.AddAsync(model);
        }

        public async Task<WorkItemReadModel> Find(IAggregateId id)
        {
            return await readDatabase.WorkItems.FirstOrDefaultAsync(x => x.Id == id.Value);
        }

        public async Task<IEnumerable<WorkItemReadModel>> FindAllAsync(
            Expression<Func<WorkItemReadModel, bool>> predicate)
        {
            return await readDatabase.WorkItems.Where(predicate).ToArrayAsync();
        }
    }
}
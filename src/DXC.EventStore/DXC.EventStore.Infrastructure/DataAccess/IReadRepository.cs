using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.ReadModel;

namespace DXC.EventStore.Infrastructure.DataAccess
{
    public interface IReadRepository<TReadModel>
        where TReadModel : IReadModel
    {
        Task Insert(TReadModel model);
        Task<TReadModel> Find(IAggregateId id);
        Task<IEnumerable<TReadModel>> FindAllAsync(Expression<Func<TReadModel, bool>> predicate);
    }
}
using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VUserAggregateRepository : SqlServerRepository<V_UserAggregate>, IVUserAggregateRepository
    {
        public VUserAggregateRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }
    }
}

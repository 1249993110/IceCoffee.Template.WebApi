using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class RoleMenuRepository : SqlServerRepository<T_RoleMenu>, IRoleMenuRepository
    {
        public RoleMenuRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }
    }
}

using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VRoleMenuRepository : SqlServerRepository<V_RoleMenu>, IVRoleMenuRepository
    {

        public VRoleMenuRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public Task<IEnumerable<V_RoleMenu>> QueryEnabledByRoleIds(IEnumerable<Guid> roleIds)
        {
            string sql = string.Format("SELECT {0} FROM V_RoleMenu WHERE RoleId IN @RoleIds AND MenuEnabled=1", Select_Statement);
            return base.QueryAsync<V_RoleMenu>(sql, new { RoleIds = roleIds });
        }
    }
}

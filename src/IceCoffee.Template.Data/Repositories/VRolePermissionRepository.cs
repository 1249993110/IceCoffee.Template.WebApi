using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VRolePermissionRepository : SqlServerRepository<V_RolePermission>, IVRolePermissionRepository
    {
        private const string sql1 = "SELECT DISTINCT Area FROM V_RolePermission WHERE RoleName IN @RoleNames AND PermissionEnabled=1";

        public VRolePermissionRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public Task<IEnumerable<string>> QueryEnabledAreasByRoleNames(IEnumerable<string> roleNames)
        {
            return base.QueryAsync<string>(sql1, new { RoleNames = roleNames });
        }
    }
}

using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VRolePermissionRepository : SqlServerRepository<V_RolePermission>, IVRolePermissionRepository
    {
        private const string sql = "SELECT DISTINCT Area FROM V_RolePermission WHERE RoleNames IN @RoleNames";

        public VRolePermissionRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public Task<IEnumerable<string>> QueryAreasByRoleNames(IEnumerable<string> roleNames, bool isEnabled = true)
        {
            return base.QueryAsync<string>(sql + (isEnabled ? " PermissionEnabled=1" : string.Empty), new { RoleNames = roleNames });
        }
    }
}

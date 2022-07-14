using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VRolePermissionRepository : PostgreSqlRepository<V_RolePermission>, IVRolePermissionRepository
    {
        public VRolePermissionRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }
    }
}

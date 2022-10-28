using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VUserRoleRepository : SqlServerRepository<V_UserRole>, IVUserRoleRepository
    {
        public VUserRoleRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public async Task<IEnumerable<Guid>> QueryUserIdByRoleIdOrNameAsync(string roleIdOrName)
        {
            IEnumerable<V_UserRole> vUserRoles;
            if (Guid.TryParse(roleIdOrName, out Guid id))
            {
                vUserRoles = await base.QueryByIdAsync("RoleId", id);
            }
            else
            {
                vUserRoles = await base.QueryByIdAsync("RoleName", roleIdOrName);
            }

            return vUserRoles.Select(s => s.UserId);
        }
    }
}

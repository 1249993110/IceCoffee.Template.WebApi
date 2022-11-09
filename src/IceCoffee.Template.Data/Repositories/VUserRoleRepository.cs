using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VUserRoleRepository : SqlServerRepository<V_UserRole>, IVUserRoleRepository
    {
        private const string sql = "SELECT RoleName FROM V_UserRole WHERE UserId=@UserId";

        public VUserRoleRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public Task<IEnumerable<string>> QueryRoleNamesByUserId(Guid userId, bool isEnabled = true)
        {
            return base.QueryAsync<string>(sql + (isEnabled ? " RoleEnabled=1" : string.Empty), new { UserId = userId });
        }
    }
}

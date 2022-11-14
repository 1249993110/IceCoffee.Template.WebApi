using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class VUserRoleRepository : SqlServerRepository<V_UserRole>, IVUserRoleRepository
    {
        private const string sql1 = "SELECT RoleId FROM V_UserRole WHERE UserId=@UserId AND RoleEnabled=1";
        private const string sql2 = "SELECT RoleName FROM V_UserRole WHERE UserId=@UserId AND RoleEnabled=1";

        public VUserRoleRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public Task<IEnumerable<Guid>> QueryEnabledRoleIdsByUserId(Guid userId)
        {
            return base.QueryAsync<Guid>(sql1, new { UserId = userId });
        }

        public Task<IEnumerable<string>> QueryEnabledRoleNamesByUserId(Guid userId)
        {
            return base.QueryAsync<string>(sql2, new { UserId = userId });
        }
    }
}

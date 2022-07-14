using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class UserRoleRepository : PostgreSqlRepository<T_UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }
    }
}

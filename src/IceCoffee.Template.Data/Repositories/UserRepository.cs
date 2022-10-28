using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class UserRepository : SqlServerRepository<T_User>, IUserRepository
    {
        public UserRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public async Task<T_User> QueryByLoginNameAsync(string loginName)
        {
            var result = await base.QueryAsync("Name=@LoginName OR PhoneNumber=@LoginName", 
                param: new
                {
                    LoginName = loginName
                });

            return result.FirstOrDefault();
        }
    }
}

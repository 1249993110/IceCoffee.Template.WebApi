using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class UserRepository : PostgreSqlRepository<T_User>, IUserRepository
    {
        public UserRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public async Task<T_User> QueryByLoginNameAsync(string loginName)
        {
            var result = await base.QueryAsync("name=@LoginName OR phone_number=@LoginName LIMIT 1", 
                param: new
                {
                    LoginName = loginName
                });

            return result.FirstOrDefault();
        }
    }
}

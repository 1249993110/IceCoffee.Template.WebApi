using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class UserRepository : SQLiteRepository<T_User>, IUserRepository
    {
        public UserRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public async Task<T_User> QueryByLoginNameAsync(string loginName)
        {
            var result = await base.QueryAsync("Name=@LoginName OR PhoneNumber=@LoginName LIMIT 1", 
                param: new
                {
                    LoginName = loginName
                });

            return result.FirstOrDefault();
        }
    }
}

using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;
using System.Text.RegularExpressions;

namespace IceCoffee.Template.Data.Repositories
{
    public class UserRepository : SqlServerRepository<T_User>, IUserRepository
    {
        public UserRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public async Task<T_User> QueryByLoginNameAsync(string loginName)
        {
            IEnumerable<T_User> users;

            // 手机号
            if(loginName.Length == 11 && Regex.IsMatch(loginName, @"^(1)\d{10}$"))
            {
                users = await base.QueryAsync("PhoneNumber=@LoginName",
                param: new
                {
                    LoginName = loginName
                });
            }
            // 邮件
            else if(loginName.Contains('@'))
            {
                users = await base.QueryAsync("Email=@LoginName",
                param: new
                {
                    LoginName = loginName
                });
            }
            else
            {
                users = await base.QueryAsync("Name=@LoginName",
                param: new
                {
                    LoginName = loginName
                });
            }

            return users.FirstOrDefault();
        }
    }
}

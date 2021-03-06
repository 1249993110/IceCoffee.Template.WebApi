using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class RefreshTokenRepository : PostgreSqlRepository<T_RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }
    }
}

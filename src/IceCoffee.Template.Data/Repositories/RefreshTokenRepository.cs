using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;

namespace IceCoffee.Template.Data.Repositories
{
    public class RefreshTokenRepository : SqlServerRepository<T_RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }
    }
}

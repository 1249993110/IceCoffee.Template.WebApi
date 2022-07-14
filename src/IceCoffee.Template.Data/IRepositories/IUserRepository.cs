using IceCoffee.Template.Data.Entities;

namespace IceCoffee.Template.Data.IRepositories
{
    public interface IUserRepository : IRepository<T_User>
    {
        Task<T_User> QueryByLoginNameAsync(string loginName);
    }
}

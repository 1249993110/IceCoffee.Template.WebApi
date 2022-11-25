using IceCoffee.Template.Data.Entities;

namespace IceCoffee.Template.Data.IRepositories
{
    public interface IVUserRepository : IRepository<V_User>
    {
        Task<IEnumerable<Guid>> QueryEnabledRoleIdsByUserId(Guid userId);

        Task<IEnumerable<string>> QueryEnabledRoleNamesByUserId(Guid userId);
    }
}

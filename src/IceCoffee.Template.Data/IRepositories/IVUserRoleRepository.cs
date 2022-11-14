using IceCoffee.Template.Data.Entities;

namespace IceCoffee.Template.Data.IRepositories
{
    public interface IVUserRoleRepository : IRepository<V_UserRole>
    {
        Task<IEnumerable<Guid>> QueryEnabledRoleIdsByUserId(Guid userId);

        Task<IEnumerable<string>> QueryEnabledRoleNamesByUserId(Guid userId);
    }
}

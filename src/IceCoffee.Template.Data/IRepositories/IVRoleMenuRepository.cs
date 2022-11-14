using IceCoffee.Template.Data.Entities;

namespace IceCoffee.Template.Data.IRepositories
{
    public interface IVRoleMenuRepository : IRepository<V_RoleMenu>
    {
        Task<IEnumerable<V_RoleMenu>> QueryEnabledByRoleIds(IEnumerable<Guid> roleIds);
    }
}

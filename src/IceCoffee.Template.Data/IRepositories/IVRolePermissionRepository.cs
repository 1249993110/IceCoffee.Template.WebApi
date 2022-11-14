using IceCoffee.Template.Data.Entities;

namespace IceCoffee.Template.Data.IRepositories
{
    public interface IVRolePermissionRepository : IRepository<V_RolePermission>
    {
        Task<IEnumerable<string>> QueryEnabledAreasByRoleNames(IEnumerable<string> roleNames);
    }
}

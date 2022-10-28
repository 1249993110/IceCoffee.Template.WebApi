using IceCoffee.DbCore;

namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 角色关联的菜单管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class RoleMenusController : ApiControllerBase
    {
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMenuRepository _menuRepository;

        public RoleMenusController(
            IRoleMenuRepository roleMenuRepository,
            IRoleRepository roleRepository,
            IMenuRepository menuRepository)
        {
            _roleMenuRepository = roleMenuRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 获取角色关联的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<Response<IEnumerable<Guid>>> Get([FromRoute, Required, MinLength(1)] Guid roleId)
        {
            var entities = await _roleMenuRepository.QueryByIdAsync("Fk_RoleId", roleId);

            return SucceededResult(entities.Select(e => e.MenuId));
        }

        /// <summary>
        /// 修改角色关联的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        [HttpPut("{roleId}")]
        public async Task<Response> Put([FromRoute] Guid roleId, [FromBody, Required] Guid[] menuIds)
        {
            int count = await _roleRepository.QueryRecordCountAsync("Id=@Id", new { Id = roleId });
            if (count == 0)
            {
                return FailedResult($"修改失败, 角色Id: {roleId} 不存在");
            }

            if (menuIds.Length > 0)
            {
                var entities = new List<T_RoleMenu>();
                foreach (var menuId in menuIds)
                {
                    entities.Add(new T_RoleMenu()
                    {
                        RoleId = roleId,
                        MenuId = menuId
                    });
                }

                UnitOfWork.Default.EnterContext();
                try
                {
                    _roleMenuRepository.DeleteById("Fk_RoleId", roleId);
                    _roleMenuRepository.InsertBatch(entities);

                    UnitOfWork.Default.SaveChanges();
                }
                catch (Exception)
                {
                    UnitOfWork.Default.Rollback();
                    throw;
                }
            }
            else
            {
                await _roleMenuRepository.DeleteByIdAsync("Fk_RoleId", roleId);
            }

            return SucceededResult();
        }
    }
}
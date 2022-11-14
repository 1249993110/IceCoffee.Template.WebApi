using IceCoffee.DbCore;
using Mapster;

namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class RolesController : ApiControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleMenuRepository _roleMenuRepository; 
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RolesController(
            IRoleRepository roleRepository,
            IRoleMenuRepository roleMenuRepository,
            IRolePermissionRepository rolePermissionRepository,
            IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 通过Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<Response<T_Role>> Get([FromRoute] Guid roleId)
        {
            var entity = (await _roleRepository.QueryByIdAsync("Id", roleId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult("获取失败");
            }

            return SucceededResult(entity);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<IEnumerable<T_Role>>> Get()
        {
            var entities = await _roleRepository.QueryAllAsync();
            return SucceededResult(entities);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Post([FromBody] RoleAddModel model)
        {
            var entity = model.Adapt<T_Role>();
            entity.Id = Guid.NewGuid();
            entity.CreatorId = UserInfo.UserId.ToGuidNullable();

            await _roleRepository.InsertAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Response> Put([FromBody] RoleEditModel model)
        {
            var roleId = model.Id;
            var entity = (await _roleRepository.QueryByIdAsync("Id", roleId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult($"修改失败, 角色Id: {roleId} 不存在");
            }

            model.Adapt(entity);
            entity.ModifiedDate = DateTime.Now;
            entity.ModifierId = UserInfo.UserId.ToGuidNullable();

            await _roleRepository.UpdateAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        public async Task<Response> Delete([FromRoute] Guid roleId)
        {
            int count = await _roleRepository.DeleteByIdAsync("Id", roleId);
            if (count != 1)
            {
                return FailedResult($"删除失败, 角色Id: {roleId} 不存在");
            }

            return SucceededResult();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Response> Delete([FromBody, MinLength(1)] Guid[] roleIds)
        {
            await _roleRepository.DeleteBatchByIdsAsync("Id", roleIds, true);
            return SucceededResult();
        }

        /// <summary>
        /// 获取角色关联的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}/Menus")]
        public async Task<Response<IEnumerable<Guid>>> GetMenus([FromRoute] Guid roleId)
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
        [HttpPut("{roleId}/Menus")]
        public async Task<Response> PutMenus([FromRoute] Guid roleId, [FromBody, MinLength(1)] Guid[] menuIds)
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

                try
                {
                    UnitOfWork.Default.EnterContext();

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

        /// <summary>
        /// 获取角色关联的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}/Permissions")]
        public async Task<Response<IEnumerable<Guid>>> GetPermissions([FromRoute] Guid roleId)
        {
            var entities = await _rolePermissionRepository.QueryByIdAsync("Fk_RoleId", roleId);

            return SucceededResult(entities.Select(e => e.PermissionId));
        }

        /// <summary>
        /// 修改角色关联的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        [HttpPut("{roleId}/Permissions")]
        public async Task<Response> PutPermissions([FromRoute] Guid roleId, [FromBody, MinLength(1)] Guid[] permissionIds)
        {
            int count = await _roleRepository.QueryRecordCountAsync("Id=@Id", new { Id = roleId });
            if (count == 0)
            {
                return FailedResult($"修改失败, 角色Id: {roleId} 不存在");
            }

            if (permissionIds.Length > 0)
            {
                var entities = new List<T_RolePermission>();
                foreach (var permissionId in permissionIds)
                {
                    entities.Add(new T_RolePermission()
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    });
                }

                try
                {
                    UnitOfWork.Default.EnterContext();

                    _rolePermissionRepository.DeleteById("Fk_RoleId", roleId);
                    _rolePermissionRepository.InsertBatch(entities);

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
                await _rolePermissionRepository.DeleteByIdAsync("Fk_RoleId", roleId);
            }

            return SucceededResult();
        }
    }
}
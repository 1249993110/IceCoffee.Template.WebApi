using IceCoffee.DbCore;

namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 角色关联的权限管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class RolePermissionsController : ApiControllerBase
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RolePermissionsController(IRolePermissionRepository rolePermissionRepository, IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 获取角色关联的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<Response<IEnumerable<Guid>>> Get([FromRoute] Guid roleId)
        {
            var entities = await _rolePermissionRepository.QueryByIdAsync("fk_role_id", roleId);

            return SucceededResult(entities.Select(e => e.PermissionId));
        }

        /// <summary>
        /// 修改角色关联的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        [HttpPut("{roleId}")]
        public async Task<Response> Put([FromRoute] Guid roleId, [FromBody, Required] Guid[] permissionIds)
        {
            int count = await _roleRepository.QueryRecordCountAsync("id=@Id", new { Id = roleId });
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

                UnitOfWork.Default.EnterContext();
                try
                {
                    _rolePermissionRepository.DeleteById("fk_role_id", roleId);
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
                await _rolePermissionRepository.DeleteByIdAsync("fk_role_id", roleId);
            }

            return SucceededResult();
        }
    }
}
namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 角色下的用户管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]/[action]")]
    public class RoleUsersController : ApiControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IVUserRoleRepository _vUserRoleRepository;

        public RoleUsersController(IUserRoleRepository userRoleRepository, IVUserRoleRepository vUserRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _vUserRoleRepository = vUserRoleRepository;
        }

        /// <summary>
        /// 通过角色Id获取用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<IEnumerable<string>>> GetByRoleId([FromQuery, Required] string roleId)
        {
            var entities = await _userRoleRepository.QueryByIdAsync("fk_role_id", roleId);
            return SucceededResult(entities.Select(s => s.UserId));
        }

        /// <summary>
        /// 通过角色名称获取用户
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<IEnumerable<string>>> GetByRoleName([FromQuery, Required] string roleName)
        {
            var entities = await _vUserRoleRepository.QueryByIdAsync("role_name", roleName);
            return SucceededResult(entities.Select(s => s.UserId));
        }
    }
}
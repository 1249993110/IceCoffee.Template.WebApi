namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 角色下的用户管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
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
        /// 通过角色Id或名称获取用户
        /// </summary>
        /// <param name="roleIdOrName"></param>
        /// <returns></returns>
        [HttpGet("{roleIdOrName}")]
        public async Task<Response<IEnumerable<Guid>>> GetByRoleId([FromRoute] string roleIdOrName)
        {
            var data = await _vUserRoleRepository.QueryUserIdByRoleIdOrNameAsync(roleIdOrName);
            return SucceededResult(data);
        }
    }
}
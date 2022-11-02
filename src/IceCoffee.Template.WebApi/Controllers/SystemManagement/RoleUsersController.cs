using IceCoffee.Template.Data.Entities;

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

        public RoleUsersController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 通过角色Id获取用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<Response<IEnumerable<Guid>>> GetByRoleId([FromRoute] Guid roleId)
        {
            var entities = await _userRoleRepository.QueryByIdAsync("Fk_RoleId", roleId);
            return SucceededResult(entities.Select(s => s.UserId));
        }
    }
}
using IceCoffee.DbCore;

namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 用户关联的角色管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class UserRolesController : ApiControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;

        public UserRolesController(IUserRoleRepository userRoleRepository, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取用户关联的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<Response<IEnumerable<string>>> Get([FromRoute] string userId)
        {
            var entities = await _userRoleRepository.QueryByIdAsync("fk_user_id", userId);
            return SucceededResult(entities.Select(s => s.RoleId));
        }

        /// <summary>
        /// 修改用户关联的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<Response> Put([FromRoute] string userId, [FromBody, Required] string[] roleIds)
        {
            int count = await _userRepository.QueryRecordCountAsync("id=@Id", new { Id = userId });
            if (count == 0)
            {
                return FailedResult($"修改失败, 用户Id: {userId} 不存在");
            }

            if (roleIds.Length > 0)
            {
                var entities = new List<T_UserRole>();
                foreach (var roleId in roleIds)
                {
                    entities.Add(new T_UserRole()
                    {
                        UserId = userId,
                        RoleId = roleId
                    });
                }

                UnitOfWork.Default.EnterContext();
                try
                {
                    _userRoleRepository.DeleteById("id", userId);
                    _userRoleRepository.InsertBatch(entities);

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
                await _userRoleRepository.DeleteByIdAsync("id", userId);
            }

            return SucceededResult();
        }
    }
}
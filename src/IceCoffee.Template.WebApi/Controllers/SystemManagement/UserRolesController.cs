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
        public async Task<Response<IEnumerable<Guid>>> Get([FromRoute] Guid userId)
        {
            var entities = await _userRoleRepository.QueryByIdAsync("Fk_UserId", userId);
            return SucceededResult(entities.Select(s => s.RoleId));
        }

        /// <summary>
        /// 修改用户关联的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<Response> Put([FromRoute] Guid userId, [FromBody, MinLength(1)] Guid[] roleIds)
        {
            int count = await _userRepository.QueryRecordCountAsync("Id=@Id", new { Id = userId });
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

                try
                {
                    UnitOfWork.Default.EnterContext();

                    _userRoleRepository.DeleteById("Fk_UserId", userId);
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
                await _userRoleRepository.DeleteByIdAsync("Fk_UserId", userId);
            }

            return SucceededResult();
        }
    }
}
using IceCoffee.Common.Security.Cryptography;
using IceCoffee.DbCore;
using IceCoffee.Template.Data.Dtos;
using Mapster;
using System.Text.RegularExpressions;

namespace HYCX.Power.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IVUserRepository _vUserRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UsersController(IUserRepository userRepository, IVUserRepository vUserRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _vUserRepository = vUserRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<Response> Delete([FromRoute] Guid userId)
        {
            int count = await _userRepository.DeleteByIdAsync("Id", userId);
            if (count != 1)
            {
                return FailedResult($"删除失败, 用户Id: {userId} 不存在");
            }

            return SucceededResult();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Response> Delete([FromBody, MinLength(1)] string[] userIds)
        {
            await _userRepository.DeleteBatchByIdsAsync("Id", userIds, true);
            return SucceededResult();
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<Response<UserModel>> Get([FromRoute] Guid userId)
        {
            var entity = (await _vUserRepository.QueryByIdAsync("Id", userId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult("获取失败");
            }

            return SucceededResult(entity.Adapt<UserModel>());
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PaginationQueryResult<UserModel>>> Get([FromQuery] UserQueryModel model)
        {
            var queryDto = model.Adapt<UserQueryDto>();
            queryDto.KeywordMappedColumnNames = new string[] { "Name", "DisplayName", "PhoneNumber" };

            if (model.IsEnabled.HasValue)
            {
                queryDto.PreWhereBy = "IsEnabled=@IsEnabled";
            }

            if (model.RoleIds != null && model.RoleIds.Length > 0)
            {
                if(string.IsNullOrEmpty(queryDto.PreWhereBy) == false)
                {
                    queryDto.PreWhereBy += " AND ";
                }

                queryDto.PreWhereBy += "EXISTS(SELECT 1 FROM T_UserRole WHERE Fk_UserId=V_User.Id AND Fk_RoleId IN @RoleIds)";
            }

            var result = await _vUserRepository.QueryPagedAsync(queryDto);
            return PaginationQueryResult(result.Adapt<PaginationQueryResult<UserModel>>());
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Post([FromBody] UserAddModel model)
        {
            string password = StringExtension.FormBase64(model.PasswordHash);
            try
            {
                CheckPassword(password);
            }
            catch (Exception ex)
            {
                return FailedResult(ex.Message);
            }

            int count = await _userRepository.QueryRecordCountAsync("Name=@Name", new { model.Name });
            if (count != 0)
            {
                return FailedResult($"新增失败, 用户名称: {model.Name} 已存在");
            }

            count = await _userRepository.QueryRecordCountAsync("PhoneNumber=@PhoneNumber", new { model.PhoneNumber });
            if (count != 0)
            {
                return FailedResult($"修改失败, 电话号码: {model.PhoneNumber} 已存在");
            }

            var entity = model.Adapt<T_User>();
            entity.Id = Guid.NewGuid();

            PBKDF2.HashPassword(password, out string passwordHash, out string passwordSalt);
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            await _userRepository.InsertAsync(entity);

            ModifyUserRoles(entity.Id, model.RoleIds.Adapt<Guid[]>());

            return SucceededResult();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Response> Put([FromBody] UserEditModel model)
        {
            var userId = model.Id.GetValueOrDefault();
            var entity = (await _userRepository.QueryByIdAsync("Id", userId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult($"修改失败, 用户Id: {userId} 不存在");
            }

            int count = await _userRepository.QueryRecordCountAsync("Name=@Name AND Id!=@Id", new { model.Name, model.Id });
            if (count != 0)
            {
                return FailedResult($"修改失败, 用户名称: {model.Name} 已存在");
            }

            count = await _userRepository.QueryRecordCountAsync("PhoneNumber=@PhoneNumber", new { model.PhoneNumber });
            if (count != 0)
            {
                return FailedResult($"修改失败, 电话号码: {model.PhoneNumber} 已存在");
            }

            entity.Name = model.Name;
            entity.DisplayName = model.DisplayName;
            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Description = model.Description;
            entity.IsEnabled = model.IsEnabled;
            entity.Address = model.Address;

            await _userRepository.UpdateAsync(entity);

            ModifyUserRoles(userId, model.RoleIds.Adapt<Guid[]>());

            return SucceededResult();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password">密码 Base64 值</param>
        /// <returns></returns>
        [HttpPut("{userId}/Password")]
        public async Task<Response> PutPassword([FromRoute] Guid userId, [FromBody, Required] string password)
        {
            password = StringExtension.FormBase64(password);
            try
            {
                CheckPassword(password);
            }
            catch (Exception ex)
            {
                return FailedResult(ex.Message);
            }

            var entity = (await _userRepository.QueryByIdAsync("Id", userId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult($"修改失败, 用户Id: {userId} 不存在");
            }

            PBKDF2.HashPassword(password, out string passwordHash, out string passwordSalt);
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            await _userRepository.UpdateAsync(entity);

            return SucceededResult();
        }

        /// <summary>
        /// 检查密码强度
        /// </summary>
        /// <param name="password"></param>
        /// <exception cref="Exception"></exception>
        private static void CheckPassword(string password)
        {
            if (Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[.?`~!@#$%^&*()_])[A-Za-z\d.?`~!@#$%^&*()_]{8,16}$") == false)
            {
                throw new Exception("修改失败, 密码必须是8-16位英文字母、数字、字符组合");
            }
        }

        /// <summary>
        /// 获取用户关联的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/Roles")]
        public async Task<Response<IEnumerable<Guid>>> GetRoles([FromRoute] Guid userId)
        {
            var entities = await _userRoleRepository.QueryByIdAsync("Fk_UserId", userId);
            return SucceededResult(entities.Select(s => s.RoleId));
        }

        private void ModifyUserRoles(Guid userId, Guid[] roleIds)
        {
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
                _userRoleRepository.DeleteById("Fk_UserId", userId);
            }
        }

        /// <summary>
        /// 修改用户关联的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPut("{userId}/Roles")]
        public async Task<Response> PutRoles([FromRoute] Guid userId, [FromBody] Guid[] roleIds)
        {
            int count = await _userRepository.QueryRecordCountAsync("Id=@Id", new { Id = userId });
            if (count == 0)
            {
                return FailedResult($"修改失败, 用户Id: {userId} 不存在");
            }

            ModifyUserRoles(userId, roleIds);

            return SucceededResult();
        }

        /// <summary>
        /// 修改用户启用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPut("{userId}/Enabled")]
        public async Task<Response> PutEnabled([FromRoute] Guid userId, [FromBody, Required] bool isEnabled)
        {
            int count = await _userRepository.QueryRecordCountAsync("Id=@Id", new { Id = userId });
            if (count == 0)
            {
                return FailedResult($"修改失败, 用户Id: {userId} 不存在");
            }

            await _userRepository.UpdateColumnByIdAsync("Id", userId, "IsEnabled", isEnabled);

            return SucceededResult();
        }
    }
}
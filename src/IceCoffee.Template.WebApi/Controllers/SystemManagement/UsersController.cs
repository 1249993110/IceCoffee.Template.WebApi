using IceCoffee.Common.Security.Cryptography;
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

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<Response<UserModel>> Get([FromRoute] Guid userId)
        {
            var entity = (await _userRepository.QueryByIdAsync("Id", userId)).FirstOrDefault();
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
        public async Task<Response<PaginationQueryResult<UserModel>>> Get([FromQuery] PaginationQueryModel model)
        {
            var dto = await _userRepository.QueryPagedAsync(model.Adapt<PaginationQueryDto>(), new string[] { "Name", "DisplayName" });
            return PaginationQueryResult(dto.Adapt<PaginationQueryResult<UserModel>>());
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

            var entity = model.Adapt<T_User>();
            entity.Id = Guid.NewGuid();

            PBKDF2.HashPassword(password, out string passwordHash, out string passwordSalt);
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            await _userRepository.InsertAsync(entity);
            return SucceededResult();
        }

        private static void CheckPassword(string password)
        {
            if (Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[.?`~!@#$%^&*()_])[A-Za-z\d.?`~!@#$%^&*()_]{8,16}$") == false)
            {
                throw new Exception("修改失败, 密码必须是8-16位英文字母、数字、字符组合");
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Response> Put([FromBody] UserEditModel model)
        {
            var userId = model.Id;
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

            entity.Name = model.Name;
            entity.DisplayName = model.DisplayName;
            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Description = model.Description;
            entity.IsEnabled = model.IsEnabled;
            entity.Address = model.Address;

            await _userRepository.UpdateAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<Response> PutPassword([FromRoute] Guid userId,[FromBody] PasswordModel model)
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
    }
}
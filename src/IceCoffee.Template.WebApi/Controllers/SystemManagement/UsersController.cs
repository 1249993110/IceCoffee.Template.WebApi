//using HYCX.AspNetCore.Controllers;
//using HYCX.AspNetCore.Models;
//using HYCX.Common.Utils;
//using IceCoffee.Template.WebApi.Data.Entities;
//using IceCoffee.Template.WebApi.Data.IRepositories;
//using IceCoffee.Template.WebApi.Models;
//using IceCoffee.Template.WebApi.Models.SystemManagement;
//using IceCoffee.AspNetCore;
//using IceCoffee.AspNetCore.Models;
//using IceCoffee.AspNetCore.Models.Primitives;
//using IceCoffee.Common.Extensions;
//using IceCoffee.DbCore.Primitives.Dto;
//using Mapster;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
//{
//    /// <summary>
//    /// 用户管理
//    /// </summary>
//    [Area(nameof(SystemManagement))]
//    [Route("api/[area]/[controller]")]
//    public class UsersController : ApiControllerBase
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IVUserAppRepository _vUserAppRepository;

//        public UsersController(IUserRepository userRepository, IVUserAppRepository vUserAppRepository)
//        {
//            _userRepository = userRepository;
//            _vUserAppRepository = vUserAppRepository;
//        }

//        /// <summary>
//        /// 获取用户
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        [HttpGet("{userId}")]
//        [SucceededResponseType(typeof(Resp_User))]
//        public async Task<IResponse> Get([FromRoute] Guid userId)
//        {
//            var entity = (await _userRepository.QueryByIdAsync(nameof(T_User.Id), userId)).FirstOrDefault();
//            if (entity == null)
//            {
//                return FailedResult("获取失败");
//            }

//            return SucceededResult(entity.Adapt<Resp_User>());
//        }

//        /// <summary>
//        /// 获取用户
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [SucceededResponseType(typeof(Resp_PaginationQuery<Resp_User>))]
//        public async Task<IResponse> Get([FromQuery] Req_User_Query param)
//        {
//            PaginationResultDto dto = null;
//            if (param.AppId.HasValue)
//            {
//                dto = await _vUserAppRepository.QueryPagedAsync(param.Adapt<PaginationQueryDto>(), param.AppId.Value);
//            }
//            else
//            {
//                dto = await _userRepository.QueryPagedAsync(param.Adapt<PaginationQueryDto>(), new string[] { "Name", "DisplayName" });
//            }

//            return PaginationQueryResult(dto.Items.Adapt<IEnumerable<Resp_User>>(), dto.Total);
//        }

//        /// <summary>
//        /// 新增用户
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public async Task<IResponse> Post([FromBody] Req_User_Add user)
//        {
//            uint count = await _userRepository.QueryRecordCountAsync("Name=@Name", new { user.Name });
//            if (count != 0)
//            {
//                return FailedResult($"新增失败, 用户名称: {user.Name} 已存在");
//            }

//            var entity = user.Adapt<T_User>();
//            entity.Id = Guid.NewGuid();
//            entity.PasswordHash = CryptoTools.Des3Encrypt(StringExtension.FormBase64(user.PasswordHash));

//            await _userRepository.InsertAsync(entity);
//            return SucceededResult();
//        }

//        /// <summary>
//        /// 修改用户
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        [HttpPut]
//        public async Task<IResponse> Put([FromBody] Req_User_Edit user)
//        {
//            var userId = user.Id;
//            var entity = (await _userRepository.QueryByIdAsync(nameof(T_User.Id), userId)).FirstOrDefault();
//            if (entity == null)
//            {
//                return FailedResult($"修改失败, 用户Id: {userId} 不存在");
//            }

//            uint count = await _userRepository.QueryRecordCountAsync("Name=@Name AND Id!=@Id", new { user.Name, user.Id });
//            if (count != 0)
//            {
//                return FailedResult($"修改失败, 用户名称: {user.Name} 已存在");
//            }

//            user.Adapt(entity);
//            if(string.IsNullOrEmpty(user.PasswordHash) == false)
//            {
//                entity.PasswordHash = CryptoTools.Des3Encrypt(StringExtension.FormBase64(user.PasswordHash));
//            }
            
//            await _userRepository.UpdateAsync(entity);
//            return SucceededResult();
//        }

//        /// <summary>
//        /// 删除用户
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        [HttpDelete("{userId}")]
//        public async Task<IResponse> Delete([FromRoute] Guid userId)
//        {
//            int count = await _userRepository.DeleteByIdAsync(nameof(T_User.Id), userId);
//            if (count != 1)
//            {
//                return FailedResult($"删除失败, 用户Id: {userId} 不存在");
//            }

//            return SucceededResult();
//        }

//        /// <summary>
//        /// 删除用户
//        /// </summary>
//        /// <param name="userIds"></param>
//        /// <returns></returns>
//        [HttpDelete]
//        public async Task<IResponse> Delete([FromBody, Required, MinLength(1)] Guid[] userIds)
//        {
//            int count = await _userRepository.DeleteBatchByIdsAsync(nameof(T_User.Id), userIds, true);
//            if (count != userIds.Length)
//            {
//                return FailedResult("删除失败");
//            }

//            return SucceededResult();
//        }
//    }
//}
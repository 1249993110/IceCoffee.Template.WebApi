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

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<Response<T_Role>> Get([FromRoute] string roleId)
        {
            var entity = (await _roleRepository.QueryByIdAsync("Id", roleId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult("获取失败");
            }

            return SucceededResult(entity);
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PaginationQueryResult<T_Role>>> Get([FromQuery] PaginationQueryModel models)
        {
            var dto = await _roleRepository.QueryPagedAsync(models.Adapt<PaginationQueryDto>(), "Name");
            return PaginationQueryResult(dto.Adapt<PaginationQueryResult<T_Role>>());
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
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatorId = UserInfo.UserId;

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
            entity.ModifierId = UserInfo.UserId;

            await _roleRepository.UpdateAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        public async Task<Response> Delete([FromRoute] string roleId)
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
        public async Task<Response> Delete([FromBody, Required, MinLength(1)] string[] roleIds)
        {
            await _roleRepository.DeleteBatchByIdsAsync("Id", roleIds, true);
            return SucceededResult();
        }
    }
}
using Mapster;

namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class PermissionsController : ApiControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionsController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet("{permissionId}")]
        public async Task<Response<T_Permission>> Get([FromRoute] Guid permissionId)
        {
            var entity = (await _permissionRepository.QueryByIdAsync("Id", permissionId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult("获取失败");
            }

            return SucceededResult(entity);
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PaginationQueryResult<T_Permission>>> Get([FromQuery] PaginationQueryModel models)
        {
            var dto = await _permissionRepository.QueryPagedAsync(models.Adapt<PaginationQueryDto>());
            return PaginationQueryResult(dto.Adapt<PaginationQueryResult<T_Permission>>());
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Post([FromBody] PermissionAddModel model)
        {
            var entity = model.Adapt<T_Permission>();
            entity.Id = Guid.NewGuid();
            entity.CreatorId = UserInfo.UserId.ToGuidNullable();

            await _permissionRepository.InsertAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Response> Put([FromBody] PermissionEditModel model)
        {
            var permissionId = model.Id;
            var entity = (await _permissionRepository.QueryByIdAsync("Id", permissionId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult($"修改失败, 权限Id: {permissionId} 不存在");
            }

            model.Adapt(entity);
            entity.ModifiedDate = DateTime.Now;
            entity.ModifierId = UserInfo.UserId.ToGuidNullable();

            await _permissionRepository.UpdateAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpDelete("{permissionId}")]
        public async Task<Response> Delete([FromRoute] Guid permissionId)
        {
            int count = await _permissionRepository.DeleteByIdAsync("Id", permissionId);
            if (count != 1)
            {
                return FailedResult($"删除失败, 权限Id: {permissionId} 不存在");
            }

            return SucceededResult();
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Response> Delete([FromBody, MinLength(1)] Guid[] permissionIds)
        {
            await _permissionRepository.DeleteBatchByIdsAsync("Id", permissionIds, true);
            return SucceededResult();
        }
    }
}
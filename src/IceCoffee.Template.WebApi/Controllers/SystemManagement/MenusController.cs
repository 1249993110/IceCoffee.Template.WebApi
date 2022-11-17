using IceCoffee.Template.WebApi.Extensions;
using Mapster;

namespace IceCoffee.Template.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Area(nameof(SystemManagement))]
    [Route("api/[area]/[controller]")]
    public class MenusController : ApiControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenusController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet("{menuId}")]
        public async Task<Response<T_Menu>> Get([FromRoute] Guid menuId)
        {
            var entity = (await _menuRepository.QueryByIdAsync("Id", menuId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult($"获取失败, 菜单Id: {menuId} 不存在");
            }

            return SucceededResult(entity);
        }

        /// <summary>
        /// 获取树形结构的菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<IEnumerable<MenuTreeModel>>> GetTree()
        {
            var entities = await _menuRepository.QueryAllAsync();
            return SucceededResult(entities.ToTreeModel());
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response> Post([FromBody] MenuAddModel model)
        {
            var entity = model.Adapt<T_Menu>();
            entity.Id = Guid.NewGuid();
            entity.CreatorId = UserInfo.UserId.ToGuidNullable();

            await _menuRepository.InsertAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Response> Put([FromBody] MenuEditModel model)
        {
            var menuId = model.Id;
            var entity = (await _menuRepository.QueryByIdAsync("Id", menuId)).FirstOrDefault();
            if (entity == null)
            {
                return FailedResult($"修改失败, 菜单Id: {menuId} 不存在");
            }

            model.Adapt(entity);
            entity.ModifiedDate = DateTime.Now;
            entity.ModifierId = UserInfo.UserId.ToGuidNullable();

            await _menuRepository.UpdateAsync(entity);
            return SucceededResult();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpDelete("{menuId}")]
        public async Task<Response> Delete([FromRoute] Guid menuId)
        {
            int count = await _menuRepository.DeleteByIdAsync("Id", menuId);
            if (count != 1)
            {
                return FailedResult($"删除失败, 菜单Id: {menuId} 不存在");
            }

            await _menuRepository.DeleteByIdAsync("ParentId", menuId);

            return SucceededResult();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Response> Delete([FromBody, MinLength(1)] Guid[] menuIds)
        {
            await _menuRepository.DeleteBatchByIdsAsync("Id", menuIds, true);
            await _menuRepository.DeleteBatchByIdsAsync("ParentId", menuIds, true);
            return SucceededResult();
        }

        /// <summary>
        /// 修改权限启用
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPut("{menuId}/Enabled")]
        public async Task<Response> PutEnabled([FromRoute] Guid menuId, [FromBody, Required] bool isEnabled)
        {
            int count = await _menuRepository.QueryRecordCountAsync("Id=@Id", new { Id = menuId });
            if (count == 0)
            {
                return FailedResult($"修改失败, 菜单Id: {menuId} 不存在");
            }

            await _menuRepository.UpdateColumnByIdAsync("Id", menuId, "IsEnabled", isEnabled);

            return SucceededResult();
        }
    }
}
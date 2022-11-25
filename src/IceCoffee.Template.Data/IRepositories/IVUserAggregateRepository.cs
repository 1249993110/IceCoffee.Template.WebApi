using IceCoffee.DbCore.Dtos;
using IceCoffee.Template.Data.Dtos;
using IceCoffee.Template.Data.Entities;

namespace IceCoffee.Template.Data.IRepositories
{
    public interface IVUserAggregateRepository : IRepository<V_UserAggregate>
    {
        Task<PaginationResultDto<V_UserAggregate>> QueryPagedAsync(UserQueryDto queryDto);
    }
}

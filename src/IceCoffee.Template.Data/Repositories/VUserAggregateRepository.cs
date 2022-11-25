using IceCoffee.DbCore.Dtos;
using IceCoffee.Template.Data.Dtos;
using IceCoffee.Template.Data.Entities;
using IceCoffee.Template.Data.IRepositories;
using System.Reflection;

namespace IceCoffee.Template.Data.Repositories
{
    public class VUserAggregateRepository : SqlServerRepository<V_UserAggregate>, IVUserAggregateRepository
    {
        public VUserAggregateRepository(DefaultDbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        public Task<PaginationResultDto<V_UserAggregate>> QueryPagedAsync(UserQueryDto queryDto)
        {
            queryDto.KeywordMappedColumnNames = new string[] { "Name", "DisplayName", "PhoneNumber" };

            if (queryDto.IsEnabled.HasValue)
            {
                queryDto.PreWhereBy = "IsEnabled=@IsEnabled";
            }

            if (queryDto.RoleIds != null && queryDto.RoleIds.Length > 0)
            {
                if (string.IsNullOrEmpty(queryDto.PreWhereBy) == false)
                {
                    queryDto.PreWhereBy += " AND ";
                }

                queryDto.PreWhereBy += "EXISTS(SELECT 1 FROM T_UserRole WHERE FK_UserId=V_UserAggregate.Id AND FK_RoleId IN @RoleIds)";
            }

            return base.QueryPagedAsync(queryDto);
        }
    }
}

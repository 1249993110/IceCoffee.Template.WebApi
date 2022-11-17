using IceCoffee.DbCore.Dtos;
using System.ComponentModel;

namespace IceCoffee.Template.Data.Dtos
{
    public class UserQueryDto : PaginationQueryDto
    {
        public Guid[] RoleIds { get; set; }

        public bool? IsEnabled { get; set; }
    }
}
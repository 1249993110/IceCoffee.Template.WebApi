using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IceCoffee.Template.WebApi.Models
{
    public class UserAddModel
    {
        public UserAddModel(string passwordHash, Guid[] roleIds)
        {
            PasswordHash = passwordHash;
            RoleIds = roleIds;
        }

        /// <summary>
        ///
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string? DisplayName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string? Name { get; set; }
        
        /// <summary>
        ///
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }
        
        /// <summary>
        ///
        /// </summary>
        public string? PhoneNumber { get; set; }

        [Required]
        public Guid[] RoleIds { get; set; }
    }

    public class UserEditModel
    {
        /// <summary>
        ///
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string? DisplayName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public Guid? Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? PhoneNumber { get; set; }

        [Required]
        public Guid[] RoleIds { get; set; }

        public UserEditModel(Guid[] roleIds)
        {
            RoleIds = roleIds;
        }
    }

    public class UserModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? LastLoginIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string[]? RoleIds => Roles?.Split(',');

        [JsonIgnore]
        public string? Roles { get; set; }
    }

    public class UserQueryModel : PaginationQueryModel
    {
        public Guid[]? RoleIds { get; set; }

        public bool? IsEnabled { get; set; }
    }
}
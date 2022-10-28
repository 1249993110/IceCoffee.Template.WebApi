namespace IceCoffee.Template.WebApi.Models
{
    public class UserModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PhoneNumber { get; set; }

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
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? LastLoginIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool LoginEnabled { get; set; }
    }

    public class UserAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }

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
        public string PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PhoneNumber { get; set; }

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
        public bool LoginEnabled { get; set; }

        public UserAddModel(string name, string passwordHash)
        {
            Name = name;
            PasswordHash = passwordHash;
        }
    }

    public class UserEditModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string? Name { get; set; }

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
        public string? PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PhoneNumber { get; set; }

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
        public bool LoginEnabled { get; set; }

    }
}

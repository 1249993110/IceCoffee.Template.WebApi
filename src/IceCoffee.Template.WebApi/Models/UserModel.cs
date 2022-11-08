namespace IceCoffee.Template.WebApi.Models
{
    public class UserAddModel
    {
        public UserAddModel(string passwordHash)
        {
            PasswordHash = passwordHash;
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
        public bool LoginEnabled { get; set; }

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
    }

    public class PasswordModel
    {
        [Required]
        public string PasswordHash { get; set; }

        public PasswordModel(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
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
        public bool LoginEnabled { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? PhoneNumber { get; set; }
    }

    public class UserModel
    {
        /// <summary>
        ///
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Description { get; set; }

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
        public Guid Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string? LastLoginIp { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool LoginEnabled { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string? PhoneNumber { get; set; }
    }
}
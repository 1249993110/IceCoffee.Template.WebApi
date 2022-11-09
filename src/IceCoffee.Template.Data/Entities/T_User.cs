namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_User
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreInsert, IgnoreUpdate]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LockoutEndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AccessFailedCount { get; set; }

    }
}

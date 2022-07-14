namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_user")]
    public class T_User
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("password_salt")]
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("address")]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("last_login_time")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("last_login_ip")]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("login_enabled")]
        public bool LoginEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("lockout_end_date")]
        public DateTime? LockoutEndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("access_failed_count")]
        public int AccessFailedCount { get; set; }

    }
}

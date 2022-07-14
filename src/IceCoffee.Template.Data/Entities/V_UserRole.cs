namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("v_user_role")]
    public class V_UserRole
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("user_id")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("user_name")]
        public string UserName { get; set; }

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
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("role_id")]
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("role_name")]
        public string RoleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("role_enabled")]
        public bool? RoleEnabled { get; set; }

    }
}

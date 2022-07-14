namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_user_role")]
    public class T_UserRole
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("fk_user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("fk_role_id")]
        public Guid RoleId { get; set; }

    }
}

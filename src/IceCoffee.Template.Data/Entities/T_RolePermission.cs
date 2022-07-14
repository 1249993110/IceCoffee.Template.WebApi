namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_role_permission")]
    public class T_RolePermission
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("fk_role_id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("fk_permission_id")]
        public Guid PermissionId { get; set; }

    }
}

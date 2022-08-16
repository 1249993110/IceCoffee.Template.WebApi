namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("v_role_permission")]
    public class V_RolePermission
    {
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
        [Column("permission_id")]
        public Guid? PermissionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("area")]
        public string Area { get; set; }

    }
}

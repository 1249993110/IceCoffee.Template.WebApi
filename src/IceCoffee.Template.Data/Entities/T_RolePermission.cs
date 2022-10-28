namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_RolePermission
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("Fk_RoleId")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("Fk_PermissionId")]
        public Guid PermissionId { get; set; }

    }
}

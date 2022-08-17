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
        public string RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("Fk_PermissionId")]
        public string PermissionId { get; set; }

    }
}

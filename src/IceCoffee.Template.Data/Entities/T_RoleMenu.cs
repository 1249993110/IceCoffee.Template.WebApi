namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_role_menu")]
    public class T_RoleMenu
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
        [Column("fk_menu_id")]
        public Guid MenuId { get; set; }

    }
}

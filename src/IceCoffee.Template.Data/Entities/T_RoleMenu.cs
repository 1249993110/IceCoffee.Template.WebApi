namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_RoleMenu
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
        [Column("Fk_MenuId")]
        public Guid MenuId { get; set; }

    }
}

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
        [Column("FK_RoleId")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("FK_MenuId")]
        public Guid MenuId { get; set; }

    }
}

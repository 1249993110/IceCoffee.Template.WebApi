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
        public string RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("Fk_MenuId")]
        public string MenuId { get; set; }

    }
}

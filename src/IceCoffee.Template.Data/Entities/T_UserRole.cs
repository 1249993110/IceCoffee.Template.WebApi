namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_UserRole
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("Fk_UserId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("Fk_RoleId")]
        public Guid RoleId { get; set; }

    }
}

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
        [Column("FK_UserId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("FK_RoleId")]
        public Guid RoleId { get; set; }

    }
}

namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_role")]
    public class T_Role
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("created_date"), IgnoreInsert, IgnoreUpdate]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("creator_id")]
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("modifier_id")]
        public Guid? ModifierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

    }
}

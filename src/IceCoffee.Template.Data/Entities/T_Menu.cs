namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_menu")]
    public class T_Menu
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
        [Column("created_date")]
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
        [Column("parent_id")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("url")]
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("is_external_link")]
        public bool IsExternalLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

    }
}

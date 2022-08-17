namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_Permission
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreInsert, IgnoreUpdate]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModifierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

    }
}

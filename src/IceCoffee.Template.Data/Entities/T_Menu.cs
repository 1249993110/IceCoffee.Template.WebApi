﻿namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class T_Menu
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreInsert, IgnoreUpdate]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ModifierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExternalLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

    }
}

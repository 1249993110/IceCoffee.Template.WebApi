namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class V_RoleMenu
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RoleEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? MenuParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? MenuEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsExternalLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MenuDescription { get; set; }

    }
}

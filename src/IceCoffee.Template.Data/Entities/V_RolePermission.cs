namespace IceCoffee.Template.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class V_RolePermission
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
        public Guid? PermissionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RoleEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? PermissionEnabled { get; set; }
    }
}

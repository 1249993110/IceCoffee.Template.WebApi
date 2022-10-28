namespace IceCoffee.Template.WebApi.Models
{
    public class PermissionAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string? Area { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; }
    }

    public class PermissionEditModel : PermissionAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Id { get; set; }
    }
}

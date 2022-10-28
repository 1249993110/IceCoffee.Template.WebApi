namespace IceCoffee.Template.WebApi.Models
{
    public class RoleAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }
    }

    public class RoleEditModel : RoleAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Id { get; set; }
    }
}

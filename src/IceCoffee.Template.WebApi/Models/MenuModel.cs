namespace IceCoffee.Template.WebApi.Models
{
    public class MenuAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExternalLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; }
    }

    public class MenuEditModel : MenuAddModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid? Id { get; set; }
    }
}

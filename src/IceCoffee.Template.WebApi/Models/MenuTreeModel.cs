namespace IceCoffee.Template.WebApi.Models
{
    public class MenuTreeModel
    {
        public List<MenuTreeModel>? Children { get; set; }

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string? Name { get; set; }

        public string? Icon { get; set; }

        public int Sort { get; set; }

        public string? Url { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsExternalLink { get; set; }

        public string? Description { get; set; }
    }
}

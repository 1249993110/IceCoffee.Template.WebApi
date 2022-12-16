using Mapster;

namespace IceCoffee.Template.WebApi.Extensions
{
    public static class MenuExtension
    {
        /// <summary>
        /// 转换为树形结构
        /// </summary>
        /// <param name="menus">菜单集合</param>
        public static IEnumerable<MenuTreeModel> ToTreeModel(this IEnumerable<T_Menu> menus)
        {
            var lstTreeNodes = new List<MenuTreeModel>();

            var parentMenus = menus.Where(t => t.ParentId == null).OrderBy(m => m.Sort);
            foreach (var item in parentMenus)
            {
                var node = ParseTreeNode(menus, item);
                lstTreeNodes.Add(node);
            }

            return lstTreeNodes;
        }

        /// <summary>
        /// 解析树形节点
        /// </summary>
        /// <param name="menus">菜单集合</param>
        /// <param name="item">当前菜单节点</param>
        private static MenuTreeModel ParseTreeNode(IEnumerable<T_Menu> menus, T_Menu item)
        {
            var treenode = item.Adapt<MenuTreeModel>();

            var subitems = menus.Where(p => p.ParentId == item.Id && p.Id != item.Id);

            if (subitems.Any())
            {
                treenode.Children = new List<MenuTreeModel>();
                foreach (var subitem in subitems.OrderBy(m => m.Sort))
                {
                    var submodule = ParseTreeNode(menus, subitem);
                    treenode.Children.Add(submodule);
                }
            }

            return treenode;
        }

        /// <summary>
        /// 转换为树形结构
        /// </summary>
        /// <param name="menus">菜单集合</param>
        public static IEnumerable<MenuTreeModel> ToTreeModel(this IEnumerable<V_RoleMenu> menus)
        {
            var lstTreeNodes = new List<MenuTreeModel>();

            var parentMenus = menus.Where(t => t.MenuParentId == null).OrderBy(m => m.Sort);
            foreach (var item in parentMenus)
            {
                var node = ParseTreeNode(menus, item);
                lstTreeNodes.Add(node);
            }

            return lstTreeNodes;
        }

        /// <summary>
        /// 解析树形节点
        /// </summary>
        /// <param name="menus">菜单集合</param>
        /// <param name="item">当前菜单节点</param>
        private static MenuTreeModel ParseTreeNode(IEnumerable<V_RoleMenu> menus, V_RoleMenu item)
        {
            var treenode = new MenuTreeModel()
            {
                Id = item.MenuId.GetValueOrDefault(),
                Name = item.MenuName,
                ParentId = item.MenuParentId,
                IsEnabled = item.MenuEnabled.GetValueOrDefault(),
                Description = item.MenuDescription,
                Icon = item.Icon,
                IsExternalLink = item.IsExternalLink.GetValueOrDefault(),
                Sort = item.Sort.GetValueOrDefault(),
                Url = item.Url,
                Children = null
            };

            var subitems = menus.Where(p => p.MenuParentId == item.MenuId && p.MenuId != item.MenuId);

            if (subitems.Any())
            {
                treenode.Children = new List<MenuTreeModel>();
                foreach (var subitem in subitems.OrderBy(m => m.Sort))
                {
                    var submodule = ParseTreeNode(menus, subitem);
                    treenode.Children.Add(submodule);
                }
            }

            return treenode;
        }
    }
}

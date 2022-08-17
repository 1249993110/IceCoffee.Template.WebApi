using IceCoffee.Template.Data.Entities;
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

            var subitems = menus.Where(p => p.ParentId == item.Id && p.Id != item.Id).OrderBy(m => m.Sort);

            if (subitems != null && subitems.Any())
            {
                treenode.Children = new List<MenuTreeModel>();
                foreach (var subitem in subitems)
                {
                    var submodule = ParseTreeNode(menus, subitem);
                    treenode.Children.Add(submodule);
                }
            }

            return treenode;
        }
    }
}

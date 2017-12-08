using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Data;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Service
{
    public interface IMenuService : IDependency
    {
        /// <summary>
        /// 获得树列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Menu> GetTreeList();
        /// <summary>
        /// 获得菜单列表 for select
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetTreeSelectList();
        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        AjaxResult Save(Menu menu);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AjaxResult Delete(int[] ids);

        /// <summary>
        /// 获得菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Menu Get(int id);

        /// <summary>
        /// 根据用户ID获得授权列表 （包括菜单 和 按钮）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Menu> GetAuthList();

        /// <summary>
        /// 根据用户ID获得授权菜单列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<MenuTree> GetAuthMenuList();

        /// <summary>
        /// 根据URL获得用户授权菜单列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerable<Menu> GetAuthMenuList(string url, MenuType type = MenuType.Menu);
    }

    public class MenuService : IMenuService
    {
        readonly IMenuRepository menuRep;
        readonly IUserService us;
        public MenuService(IMenuRepository imr, IUserService ius)
        {
            menuRep = imr;
            us = ius;
        }

        public IEnumerable<Menu> GetTreeList()
        {
            var list = menuRep.GetTreeList();
            return GetMenuTree(0, list);
        }

        public IEnumerable<dynamic> GetTreeSelectList()
        {
            var list = GetTreeList();

            var treeList = from item in list
                           where item.TYPE < 3
                           select new
                           {
                               id = item.ID,
                               pid = item.PARENTID,
                               text = item.NAME,
                               value = item.ID
                           };
            return treeList;
        }

        public AjaxResult Save(Menu menu)
        {
            bool addFlag = menu.ID < 1;
            var user = us.GetCurrent();

            menu.CREATEBY = user.ACCOUNT;
            menu.CREATEDATE = DateTime.Now;
            menu.UPDATEBY = user.ACCOUNT;
            menu.UPDATEDATE = DateTime.Now;
            menu.ENABLED = true;

            if (addFlag)
            {
                bool flag = menuRep.Add(menu);
                if (flag) RemoveAuthListCache();
                return new AjaxResult(flag, flag ? "菜单添加成功" : "菜单添加失败");
            }
            else
            {
                bool flag = menuRep.Update(menu, m => new
                {
                    m.NAME,
                    m.CLASSNAME,
                    m.CODE,
                    m.IMG,
                    m.TARGET,
                    m.TITLE,
                    m.TYPE,
                    m.URL,
                    m.REMARK,
                    m.SORTID,
                    m.UPDATEBY,
                    m.UPDATEDATE,
                    m.PARENTID
                }, m => m.ID == menu.ID);
                if (flag) RemoveAuthListCache();
                return new AjaxResult(flag, flag ? "菜单修改成功" : "菜单修改失败");
            }
        }

        public AjaxResult Delete(int[] roleIds)
        {
            bool flag = menuRep.Delete(roleIds);
            if (flag) RemoveAuthListCache();
            return new AjaxResult(flag, flag ? "菜单删除成功" : "菜单删除失败");
        }

        public Menu Get(int id)
        {
            return menuRep.GetEntity(m => m.ID == id);
        }

        #region 授权

        public IEnumerable<Menu> GetAuthList()
        {
            var user = us.GetCurrent();
            int userId = user.ID;

            string key = string.Format(ServiceConst.UserAuthListCache, userId);
            var list = CacheHelper.Get<IEnumerable<Menu>>(key);
            if (list != null && list.Count() > 0) return list;
            list = menuRep.GetAuthList(userId);
            CacheHelper.Add(key, list);
            return list;
        }

        public IEnumerable<MenuTree> GetAuthMenuList()
        {
            var list = this.GetAuthList();
            if (list != null && list.Count() > 0)
            {
                int type = (int)MenuType.Button;
                list = list.Where(m => m.TYPE < type);
                if (list != null)
                {
                    var menu = list.FirstOrDefault(m => m.PARENTID == 0);
                    if (menu != null)
                    {
                        return GetMenuTree2(menu.ID, list);
                    }
                }
            }
            return null;
        }

        public IEnumerable<Menu> GetAuthMenuList(string url, MenuType type = MenuType.Menu)
        {
            if (string.IsNullOrEmpty(url)) return null;
            url = url.ToLower();
            var list = this.GetAuthList();
            if (list != null && list.Count() > 0)
            {
                var tempList = list.Where(m =>
                {
                    if (string.IsNullOrEmpty(m.URL)) return false;
                    return m.URL.ToLower().Contains(url);
                });
                if (type == MenuType.Menu)
                {
                    return tempList;
                }
                else if (type == MenuType.Button)
                {
                    if (tempList != null && tempList.Count() > 0)
                    {
                        int menuType = (int)type;
                        var menu = tempList.FirstOrDefault();
                        return list.Where(m => m.PARENTID == menu.ID && m.TYPE == menuType);
                    }
                }
            }
            return null;
        }

        #endregion

        private void RemoveAuthListCache()
        {
            var user = us.GetCurrent();
            int userId = user.ID;

            string key = string.Format(ServiceConst.UserAuthListCache, userId);
            CacheHelper.Remove(key);
        }

        private IEnumerable<Menu> GetMenuTree(int parentId, IEnumerable<Menu> list)
        {
            var children = list.Where(m => m.PARENTID == parentId);
            if (children != null)
            {
                List<Menu> menus = new List<Menu>();
                children = children.OrderBy(m => m.SORTID).ToList();
                foreach (var m in children)
                {
                    menus.Add(m);
                    var ms = GetMenuTree(m.ID, list);
                    if (ms != null && ms.Count() > 0) menus.AddRange(ms);
                }
                return menus;
            }
            else return null;
        }

        private IEnumerable<MenuTree> GetMenuTree2(int parentId, IEnumerable<Menu> list)
        {
            var children = list.Where(m => m.PARENTID == parentId);
            if (children != null)
            {
                List<MenuTree> menus = new List<MenuTree>();
                children = children.OrderBy(m => m.SORTID).ToList();
                foreach (var m in children)
                {
                    var tree = new MenuTree(m);
                    var ms = GetMenuTree2(m.ID, list);
                    if (ms != null && ms.Count() > 0) tree.children = ms;
                    menus.Add(tree);
                }
                return menus;
            }
            else return null;
        }
    }
}

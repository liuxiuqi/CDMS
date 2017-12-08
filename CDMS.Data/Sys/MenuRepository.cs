using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Data
{
    public interface IMenuRepository : IRepository<Menu>
    {
        /// <summary>
        /// 获得树列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Menu> GetTreeList();

        /// <summary>
        /// 根据用户ID获得授权列表 （包括菜单 和 按钮）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Menu> GetAuthList(int userId);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool Delete(int[] ids);
    }

    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public IEnumerable<Menu> GetTreeList()
        {
            sql.SelectAll();
            sql.Where(m => m.ENABLED == true);
            sql.OrderBy(m => m.PARENTID, m => m.SORTID);
            return GetList();
        }

        public bool Delete(int[] ids)
        {
            sql.In(m => m.ID, ids);

            sql.Update(new Menu() { ENABLED = false }, m => m.ENABLED);

            int count = base.Execute(sql.GetSql(), sql.GetParameters());
            return count > 0;
        }

        public IEnumerable<Menu> GetAuthList(int userId)
        {
            var roleMenuSql = base.GetSqlLam<RoleMenu>();

            var roleUserSql = base.GetSqlLam<RoleUser>();
            roleUserSql.Where(m => m.USERID == userId).Select(m => m.ROLEID);

            roleMenuSql.In(m => m.ROLEID, roleUserSql);
            roleMenuSql.SelectDistinct(m => m.MENUID);

            sql.Where(m => m.ENABLED == true).In(m => m.ID, roleMenuSql);

            return GetList();
        }
    }
}

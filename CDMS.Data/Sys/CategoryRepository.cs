using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetTreeList();

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool Delete(int[] ids);
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public IEnumerable<Category> GetTreeList()
        {
            sql.SelectAll();
            sql.Where(m => m.ENABLED == true);

            //var query = p.json.ToObject<Category>();
            //if (query != null)
            //{
            //    string key = query.NAME;
            //    if (!string.IsNullOrEmpty(key))
            //    {
            //        sql.And().Begin();
            //        sql.Or(m => m.NAME.Contains(key));
            //        sql.Or(m => m.REMARK.Contains(key));
            //        sql.End();
            //    }
            //}

            sql.OrderBy(m => m.PARENTID, m => m.SORTID);
            return GetList();
        }

        public bool Delete(int[] ids)
        {
            sql.In(m => m.ID, ids);

            sql.Update(new Category() { ENABLED = false }, m => m.ENABLED);

            int count = base.Execute();
            return count > 0;
        }
    }
}

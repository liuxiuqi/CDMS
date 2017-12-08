using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Data
{
    public interface IArticleRepository : IRepository<Article>
    {
        LayuiPaginationOut GetList(LayuiPaginationIn p);

        bool Delete(long[] ids);
    }

    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            var sqlCategory = sql.Join<Category>((s, c) => s.CATEGORYID == c.ID, aliasName: "b");
            sqlCategory.Select(m => new { CategoryName = m.NAME });
            sql.SelectAll();
            sql.Where(m => m.ENABLED == true);
            var model = p.json.ToObject<Article>();
            if (model != null)
            {
                if (model.CATEGORYID > 0) sql.And(m => m.CATEGORYID == model.CATEGORYID);
                if (!model.CODE.IsEmpty()) sql.And(m => m.CODE == model.CODE);
                if (model.STATUS > 0) sql.And(m => m.STATUS == model.STATUS);
                if (model.ISHOT) sql.And(m => m.ISHOT == model.ISHOT);
                if (model.ISRED) sql.And(m => m.ISRED == model.ISRED);
                if (model.ISTOP) sql.And(m => m.ISTOP == model.ISTOP);
                if (!model.TITLE.IsEmpty())
                {
                    sql.And().Begin();
                    sql.Or(m => m.TITLE.Contains(model.TITLE));
                    sql.Or(m => m.SUBTITLE.Contains(model.TITLE));
                    sql.Or(m => m.SUMMARY.Contains(model.TITLE));
                    sql.Or(m => m.CONTENTS.Contains(model.TITLE));
                    sql.End();
                }
            }
            sql.OrderBy(m => m.CATEGORYID, m => m.SORTID);
            var list = base.GetDynamicPageList(p, sql);
            return new LayuiPaginationOut(p, list);
        }

        public bool Delete(long[] ids)
        {
            sql.In(m => m.ID, ids);
            sql.Update(new { ENABLED = false }, m => m.ENABLED);

            int count = base.Execute();
            return count > 0 ? true : false;
        }
    }
}

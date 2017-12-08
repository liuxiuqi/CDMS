using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Data
{
    public interface IImageRepository : IRepository<Image>
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        LayuiPaginationOut GetList(LayuiPaginationIn p);

        /// <summary>
        /// 获得图片详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ImageVM Get(int id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool Delete(int[] ids);
    }

    internal class ImageRepository : RepositoryBase<Image>, IImageRepository
    {
        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            string key = p.json;
            sql.SelectAll();
            sql.Where(m => m.ENABLED == true);
            if (!key.IsEmpty())
            {
                sql.And().Begin();
                sql.Or(m => m.TITLE.Contains(key));
                sql.Or(m => m.SUMMARY.Contains(key));
                sql.End();
            }
            sql.OrderBy(m => m.ID);
            var list = base.GetPageList(p);
            return new LayuiPaginationOut(p, list);
        }

        public ImageVM Get(int id)
        {
            string sqlText = @"
SELECT * FROM dbo.SYS_IMAGE WHERE ID=@id
SELECT * FROM dbo.SYS_IMAGEDETAIL WHERE IMAGEID=@id";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("id", id);
            var reader = base.GetReader(sqlText, dic);
            ImageVM model = new ImageVM();
            if (reader != null)
            {
                model.Image = reader.Read<Image>().FirstOrDefault();
                model.Details = reader.Read<ImageDetail>();
            }
            return model;
        }

        public bool Delete(int[] ids)
        {
            return base.UseTran(() =>
            {
                sql.In(m => m.ID, ids);
                sql.Update(new { ENABLED = false }, m => m.ENABLED);
                Execute();

                var imageDetailSql = base.GetSqlLam<ImageDetail>();
                imageDetailSql.In(m => m.IMAGEID, ids);
                imageDetailSql.Update(new { ENABLED = false }, m => m.ENABLED);
                Execute(imageDetailSql);
            });
        }
    }
}

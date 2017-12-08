using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Data;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Service
{
    public interface ICategoryService : IDependency
    {
        /// <summary>
        /// 获得树列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetTreeList();

        /// <summary>
        /// 获得菜单列表 for select
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetTreeSelectList();

        /// <summary>
        /// 保存分类
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        AjaxResult Save(Category model);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AjaxResult Delete(int[] ids);

        /// <summary>
        /// 获得分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category Get(int id);

        /// <summary>
        /// 修改分类状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        AjaxResult Update(int id, int type, bool flag);
    }

    public class CategoryService : ICategoryService
    {
        readonly ICategoryRepository cateRep;
        readonly IUserService us;
        readonly ILogService log;
        public CategoryService(ICategoryRepository icr, IUserService ius, ILogService ils)
        {
            cateRep = icr;
            us = ius;
            log = ils;
            log.Type = TableType.SYS_CATEGORY;
            log.Title = "分类";
        }

        public IEnumerable<Category> GetTreeList()
        {
            return cateRep.GetTreeList();
        }

        public IEnumerable<dynamic> GetTreeSelectList()
        {
            var list = GetTreeList();

            var treeList = from item in list
                           select new
                           {
                               id = item.ID,
                               pid = item.PARENTID,
                               text = item.NAME,
                               value = item.ID
                           };
            return treeList;
        }

        public Category Get(int id)
        {
            return cateRep.GetEntity(m => m.ID == id);
        }

        public AjaxResult Delete(int[] ids)
        {
            bool flag = cateRep.Delete(ids);
            ActionType type = ActionType.SYS_DELETE;
            string msg = WebConst.GetActionMsg(type, flag);
            log.AppendDelete(msg, "分类编号", ids).AddSystem(type, ids);
            return new AjaxResult(flag, msg);
        }

        public AjaxResult Save(Category model)
        {
            var user = us.GetCurrent();
            model.CREATEBY = user.ACCOUNT;
            model.CREATEDATE = DateTime.Now;
            model.UPDATEBY = user.ACCOUNT;
            model.UPDATEDATE = model.CREATEDATE;
            model.ENABLED = true;

            bool addFlag = model.ID < 1;
            if (addFlag)
            {
                int categoryId = cateRep.Add<int>(model);
                ActionType type = ActionType.SYS_ADD;
                string msg = WebConst.GetActionMsg(type, categoryId > 0);
                log.AddSystem(type, msg, categoryId);
                return new AjaxResult(categoryId > 0, msg);
            }
            else
            {
                bool flag = cateRep.Update(model, m => new
                {
                    m.CODE,
                    m.ISNAV,
                    m.ISSPECIAL,
                    m.NAME,
                    m.PARENTID,
                    m.REMARK,
                    m.SORTID,
                    m.TARGET,
                    m.TYPE,
                    m.URL
                }, m => m.ID == model.ID);
                ActionType type = ActionType.SYS_UPDATE;
                string msg = WebConst.GetActionMsg(type, flag);
                log.AddSystem(type, msg, model.ID);
                return new AjaxResult(flag, msg);
            }
        }

        public AjaxResult Update(int id, int type, bool status)
        {
            bool flag = false;
            string msg = string.Empty;
            string title = type == 1 ? "修改ISNAV" : "修改ISSPECIAL";
            if (type == 1)
            {
                flag = cateRep.Update(new Category { ISNAV = status }, m => m.ISNAV, m => m.ID == id);
            }
            else
            {
                flag = cateRep.Update(new Category { ISSPECIAL = status }, m => m.ISSPECIAL, m => m.ID == id);
            }
            ActionType updateType = ActionType.SYS_UPDATE;
            msg = WebConst.GetActionMsg(updateType, flag);
            log.Append(title).AppendLine().Append(msg).AddSystem(updateType, objectId: id);
            return new AjaxResult(flag, msg);
        }
    }
}

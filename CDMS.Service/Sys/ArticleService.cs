using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;
using CDMS.Data;

namespace CDMS.Service
{
    public interface IArticleService : IDependency
    {
        LayuiPaginationOut GetList(LayuiPaginationIn p);

        AjaxResult Save(Article model);

        Article Get(long id);

        AjaxResult Update(int type, long id, bool flag);

        AjaxResult Delete(long[] ids);
    }

    public class ArticleService : IArticleService
    {
        readonly IArticleRepository articleRep;
        readonly ILogService log;
        readonly IUserService us;
        public ArticleService(IArticleRepository iar, ILogService ils, IUserService ius)
        {
            articleRep = iar;
            log = ils;
            us = ius;
            log.Type = TableType.SYS_LOG;
            log.Title = "文章";
        }

        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            return articleRep.GetList(p);
        }

        public Article Get(long id)
        {
            var model = articleRep.GetEntity(m => m.ID == id);
            model.CONTENTS = Uri.EscapeDataString(model.CONTENTS);
            return model;
        }

        public AjaxResult Save(Article model)
        {
            var user = us.GetCurrent();
            model.CREATEBY = user.ACCOUNT;
            model.CREATEDATE = DateTime.Now;
            model.UPDATEBY = user.ACCOUNT;
            model.UPDATEDATE = model.CREATEDATE;
            model.ENABLED = true;
            model.CONTENTS = Uri.UnescapeDataString(model.CONTENTS);
            bool addFlag = model.ID < 1;
            if (addFlag)
            {
                string code = model.CODE;
                bool existFlag = false;
                string msg = string.Empty;
                bool flag = false;
                if (!code.IsEmpty())
                {
                    existFlag = articleRep.Exist(m => m.CODE == code && m.ENABLED == true);
                }
                if (existFlag)
                {
                    msg = string.Format("存在CODE为[{0}]的文章", code);
                }
                else
                {
                    flag = articleRep.Add(model);
                    msg = flag ? "添加文章成功" : "添加文章失败";
                }
                log.AddSystem(ActionType.SYS_ADD, "添加文章", msg);
                return new AjaxResult(flag, msg);
            }
            else
            {
                string code = model.CODE;
                bool existFlag = false;
                string msg = string.Empty;
                bool flag = false;
                if (!code.IsEmpty())
                {
                    existFlag = articleRep.Exist(m => m.ID != model.ID && m.CODE == code && m.ENABLED == true);
                }
                if (existFlag)
                {
                    msg = string.Format("存在CODE为[{0}]的文章", code);
                }
                else
                {
                    flag = articleRep.UpdateExclude(model, m => new
                    {
                        m.CREATEBY,
                        m.CREATEDATE
                    }, m => m.ID == model.ID);
                    msg = flag ? "修改文章成功" : "修改文章失败";
                }
                log.AddSystem(ActionType.SYS_UPDATE, "修改文章", msg);
                return new AjaxResult(flag, msg);
            }
        }

        public AjaxResult Delete(long[] ids)
        {
            bool flag = articleRep.Delete(ids);
            ActionType type = ActionType.SYS_DELETE;
            string msg = WebConst.GetActionMsg(type, flag);
            log.AppendDelete(msg, "文章编号", ids).AddSystem(type, ids);
            return new AjaxResult(flag, msg);
        }

        public AjaxResult Update(int type, long id, bool flag)
        {
            bool updateFlag = false;
            string msg = string.Empty;
            string title = string.Empty;
            if (type == 1)
            {
                updateFlag = articleRep.Update(new Article() { ISRED = flag }, m => m.ISRED, m => m.ID == id);
                title = "修改ISRED";
            }
            else if (type == 2)
            {
                updateFlag = articleRep.Update(new Article() { ISHOT = flag }, m => m.ISHOT, m => m.ID == id);
                title = "修改ISHOT";
            }
            else if (type == 3)
            {
                updateFlag = articleRep.Update(new Article() { ISTOP = flag }, m => m.ISTOP, m => m.ID == id);
                title = "修改ISTOP";
            }
            else if (type == 4)
            {
                updateFlag = articleRep.Update(new Article() { ISDISPLAY = flag }, m => m.ISDISPLAY, m => m.ID == id);
                title = "修改ISDISPLAY";
            }
            ActionType updateType = ActionType.SYS_UPDATE;
            msg = WebConst.GetActionMsg(updateType, updateFlag);
            log.AddSystem(updateType, msg, id);
            return new AjaxResult(updateFlag, msg);
        }
    }
}

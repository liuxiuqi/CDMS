using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;
using CDMS.Data;

namespace CDMS.Service
{
    public interface IImageService : IDependency
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
        /// 保存图片信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        AjaxResult Save(Image model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        AjaxResult Delete(int[] ids);
    }

    public class ImageService : IImageService
    {
        readonly IImageRepository imageRep;
        readonly ILogService logRep;
        public ImageService(IImageRepository iir, ILogService ils)
        {
            imageRep = iir;
            logRep = ils;
            logRep.Type = TableType.SYS_IMAGE;
            logRep.Title = "图片";
        }

        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            return imageRep.GetList(p);
        }

        public ImageVM Get(int id)
        {
            return imageRep.Get(id);
        }

        public AjaxResult Save(Image model)
        {
            var user = logRep.User;
            bool addFlag = model.ID < 1;
            model.ENABLED = true;
            model.CREATEBY = user.ACCOUNT;
            model.CREATEDATE = DateTime.Now;
            model.UPDATEBY = model.CREATEBY;
            model.UPDATEDATE = model.CREATEDATE;
            if (addFlag)
            {
                int imageId = imageRep.Add<int>(model);
                bool flag = imageId > 0;
                ActionType type = ActionType.SYS_ADD;
                string msg = WebConst.GetActionMsg(type, flag);

                logRep.Append(msg).AddSystem(type, imageId);

                return new AjaxResult(flag, msg);
            }
            else
            {
                bool flag = imageRep.Update(model, m => new
                {
                    m.CODE,
                    m.SORTID,
                    m.STATUS,
                    m.SUMMARY,
                    m.TITLE,
                    m.UPDATEBY,
                    m.UPDATEDATE
                }, m => m.ID == model.ID);

                ActionType type = ActionType.SYS_UPDATE;
                string msg = WebConst.GetActionMsg(type, flag);

                logRep.Append(msg).AddSystem(type, model.ID);

                return new AjaxResult(flag, msg);
            }
        }

        public AjaxResult Delete(int[] ids)
        {
            ActionType type = ActionType.SYS_DELETE;

            bool flag = imageRep.Delete(ids);
            string msg = WebConst.GetActionMsg(type, flag);

            logRep.AppendDelete(msg, "图片ID", ids).AddSystem(type, ids);

            return new AjaxResult(flag, msg);
        }
    }
}

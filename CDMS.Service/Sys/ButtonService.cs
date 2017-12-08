using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Data;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Service
{
    public interface IButtonService : IDependency
    {
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="Button"></param>
        /// <returns></returns>
        AjaxResult Save(Button button);

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AjaxResult Delete(int[] ids);

        /// <summary>
        /// 查询按钮分页列表
        /// </summary>
        /// <param name="p">分页对象</param>
        /// <returns></returns>
        LayuiPaginationOut GetList(PaginationIn p);

        /// <summary>
        /// 获得角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Button Get(int buttonId);

        /// <summary>
        /// 添加菜单按钮
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="ids">按钮ID数组</param>
        /// <returns></returns>
        AjaxResult AddButtons(int pid, int[] ids);
    }

    public class ButtonService : IButtonService
    {
        readonly IButtonRepository buttonRep;
        readonly IUserService us;
        public ButtonService(IButtonRepository imr, IUserService ius)
        {
            buttonRep = imr;
            us = ius;
        }

        public AjaxResult Save(Button button)
        {
            bool addFlag = button.ID < 1;
            var user = us.GetCurrent();

            button.CREATEBY = user.ACCOUNT;
            button.CREATEDATE = DateTime.Now;
            button.UPDATEBY = user.ACCOUNT;
            button.UPDATEDATE = DateTime.Now;
            button.ENABLED = true;

            if (addFlag)
            {
                bool flag = buttonRep.Add(button);
                return new AjaxResult(flag, flag ? "按钮添加成功" : "按钮添加失败");
            }
            else
            {
                bool flag = buttonRep.Update(button, m => new
                {
                    m.NAME,
                    m.CLASSNAME,
                    m.CODE,
                    m.IMG,
                    m.TITLE,
                    m.TYPE,
                    m.REMARK,
                    m.SORTID,
                    m.UPDATEBY,
                    m.UPDATEDATE
                }, m => m.ID == button.ID);
                return new AjaxResult(flag, flag ? "按钮修改成功" : "按钮修改失败");
            }
        }

        public AjaxResult Delete(int[] buttonIds)
        {
            bool flag = buttonRep.Delete(buttonIds);
            return new AjaxResult(flag, flag ? "按钮删除成功" : "按钮删除失败");
        }

        public LayuiPaginationOut GetList(PaginationIn p)
        {
            return buttonRep.GetList(p);
        }

        public Button Get(int buttonId)
        {
            return buttonRep.GetEntity(m => m.ID == buttonId);
        }

        public AjaxResult AddButtons(int pid, int[] ids)
        {
            if (pid < 1) return new AjaxResult(false, "参数错误[PID]");
            var user = us.GetCurrent();
            string createBy = user.ACCOUNT;
            bool flag = buttonRep.AddButtons(pid, ids, createBy);
            return new AjaxResult(flag, flag ? "按钮分配成功" : "按钮分配失败");
        }
    }
}

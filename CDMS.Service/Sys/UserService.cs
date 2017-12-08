using CDMS.Data;
using CDMS.Entity;
using CDMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace CDMS.Service
{
    public interface IUserService : IDependency
    {
        Suggestion GetList(int top, string key);

        User GetCurrent();

        User GetUserByWWID(string wwid);

        AjaxResult Login(string eid, string pwd);

        bool IsLogin();

        AjaxResult Logout();

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="Button"></param>
        /// <returns></returns>
        AjaxResult Save(User user);

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
        User Get(int userId);
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        AjaxResult UploadUserFace();
    }

    public class UserService : IUserService
    {
        readonly IUserRepository userRep;
        public UserService(IUserRepository ur)
        {
            userRep = ur;
        }

        public Suggestion GetList(int top, string key)
        {
            var users = userRep.GetList(top, key);
            if (users != null && users.Count() > 0)
            {
                var tempList = from item in users
                               let name = string.IsNullOrEmpty(item.CNNAME) ? item.ENNAME : item.CNNAME
                               select new
                               {
                                   Email = item.EMAIL,
                                   Phone = item.PHONE,
                                   WWID = item.ACCOUNT,
                                   Name = name,
                                   value = string.Format("{0}({1})", name, item.ACCOUNT),
                               };

                return new Suggestion(tempList);
            }
            return new Suggestion(null);
        }

        public AjaxResult Login(string eid, string pwd)
        {
            string msg = string.Empty;
            bool flag = false;

            pwd = EncryptHelper.Encrypt(pwd);
            var user = userRep.GetEntity(m => m.ACCOUNT == eid && m.PWD == pwd && m.STATUS == 1);
            if (user != null && user.ID > 0)
            {
                int minutes = WebConst.UserLoginExpiredMinutes;
                string loginKey = Guid.NewGuid().ToString();
                var browser = HttpContext.Current.Request.Browser;
                UserLogin login = new UserLogin();
                login.IP = UtilityHepler.GetIPAddress();
                login.LOGINTIME = DateTime.Now;
                login.LOGINKEY = loginKey;
                login.STATUS = true;
                login.DESCRIPTION = "用户登录系统";
                login.EMPLOYEEID = eid;
                login.EXPIREDTIME = login.LOGINTIME.AddMinutes(minutes);
                login.BROWSER = string.Format("{0}_{1}", browser.Browser, browser.Version);
                userRep.Login(login);

                string loginCookieKey = WebConst.UserLoginCookieKey;
                CookieHelper.Add(loginCookieKey, loginKey, DateTimeType.Minute, minutes * 10);

                flag = true;
                string sessionKey = WebConst.UserLoginSessionKey;
                SessionHelper.Add(sessionKey, user);
            }
            else msg = "您没有权限登录该系统";

            return new AjaxResult(flag, msg);
        }

        public User GetCurrent()
        {
            string sessionKey = WebConst.UserLoginSessionKey;
            var user = SessionHelper.Get<User>(sessionKey);
            if (user != null) return user;

            string cookieKey = WebConst.UserLoginCookieKey;
            string loginKey = CookieHelper.Get(cookieKey);
            if (!loginKey.IsEmpty())
            {
                user = userRep.IsLogin(loginKey);
                if (user != null)
                {
                    SessionHelper.Add(sessionKey, user);
                }
            }
            return user;
        }

        public User GetUserByWWID(string wwid)
        {
            return userRep.GetUserByWWID(wwid);
        }

        public bool IsLogin()
        {
            var user = this.GetCurrent();
            return user != null && user.ID > 0;
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public AjaxResult Logout()
        {
            var user = this.GetCurrent();
            string loginKey = CookieHelper.Get(WebConst.UserLoginCookieKey);
            bool logout = userRep.Logout(user.ACCOUNT, loginKey);
            string msg = logout ? "退出成功" : "退出失败";
            if (logout)
            {
                string sessionKey = WebConst.UserLoginSessionKey;
                SessionHelper.Remove(sessionKey);

                string cookieKey = WebConst.UserLoginCookieKey;
                CookieHelper.Remove(cookieKey);
            }
            return new AjaxResult(logout, msg);
        }

        private string GetEID(string eid)
        {
            if (eid.IndexOf(@"\") <= 0)
            {
                bool flag = System.Text.RegularExpressions.Regex.IsMatch(eid, @"^[\d]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (!flag) eid = string.Format(@"AP\{0}", eid);
            }
            return eid;
        }

        /// <summary>
        /// 获得验证错误信息
        /// </summary>
        /// <param name="code">错误代码</param>
        /// <returns></returns>
        private string GetValidationMsg(string code)
        {
            string error = string.Empty;
            switch (code)
            {
                case "DomainAccountNotFound":
                    error = "域帐号未找到";
                    break;
                case "WWIDNotFound":
                    error = "WWID未找到";
                    break;
                case "WrongPassword":
                    error = "密码错误";
                    break;
                case "WWIDForbidden":
                    error = "WWID验证失败次数过多";
                    break;
                case "DomainAccountForbidden":
                    error = "域帐号验证失败次数过多";
                    break;
                case "IPForbidden":
                    error = "IP验证失败次数过多";
                    break;
                case "NoAuthority":
                    error = "客户端IP未被授权";
                    break;
            }
            return error;
        }

        public AjaxResult Save(User user)
        {
            bool addFlag = user.ID < 1;
            var currentUser = GetCurrent();

            user.CREATEBY = currentUser.ACCOUNT;
            user.CREATEDATE = DateTime.Now;
            user.UPDATEBY = currentUser.ACCOUNT;
            user.UPDATEDATE = DateTime.Now;
            user.STATUS = 1;

            if (addFlag)
            {
                bool flag = userRep.Add(user);
                return new AjaxResult(flag, flag ? "用户添加成功" : "用户添加失败");
            }
            else
            {
                bool flag = userRep.Update(user, m => new
                {
                    m.ACCOUNT,
                    m.CNNAME,
                    m.ENNAME,
                    m.SEX,
                    m.AGE,
                    m.PHONE,
                    m.REMARK,
                    m.THEME,
                    m.QUESTION,
                    m.ANSWER,
                    m.UPDATEBY,
                    m.UPDATEDATE,
                    m.IMG
                }, m => m.ID == user.ID);
                var current = this.GetCurrent();
                if (current.ID == user.ID)
                {
                    if (!string.IsNullOrEmpty(user.IMG))
                    {
                        //string sessionKey = WebConst.UserLoginSessionKey;
                        //SessionHelper.Remove(sessionKey);
                        current.IMG = user.IMG;
                    }
                }
                return new AjaxResult(flag, flag ? "用户修改成功" : "用户修改失败");
            }
        }

        public AjaxResult Delete(int[] userIds)
        {
            bool flag = userRep.Delete(userIds);
            return new AjaxResult(flag, flag ? "用户删除成功" : "用户删除失败");
        }

        public LayuiPaginationOut GetList(PaginationIn p)
        {
            return userRep.GetList(p);
        }

        public User Get(int userId)
        {
            return userRep.GetEntity(m => m.ID == userId);
        }

        public AjaxResult UploadUserFace()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (files == null || files.Count < 1) return new AjaxResult(false, "没有找到上传文件");

            UploadParameter p = new UploadParameter();
            string tempPath = "/UploadFiles/UserFace";
            string resultPath = "";
            try
            {
                bool success = true;
                string msg = "";
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    p.File = new HttpPostedFileWrapper(file);
                    string fileName = p.GetDefaultFileName();
                    string path = string.Format("{0}/{1}", tempPath, fileName);
                    p.SavePath = HttpContext.Current.Server.MapPath(path);
                    var result = UploadHelper.UploadFile(UploadType.IMAGE, p);
                    if (result.Status == UploadStatus.Success)
                    {
                        success = true;
                        resultPath = path;
                    }
                    else
                    {
                        success = false;
                    }
                    msg = result.Msg;
                }
                return new AjaxResult(success, msg, resultPath);
            }
            catch (Exception e)
            {
                return new AjaxResult(false, "上传失败" + e.Message);
            }
        }


    }
}

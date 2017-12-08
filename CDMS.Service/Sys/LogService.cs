using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;
using CDMS.Data;
using System.Web;

namespace CDMS.Service
{
    public interface ILogService : IDependency
    {
        /// <summary>
        /// 表名
        /// </summary>
        TableType Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 当前登录对象
        /// </summary>
        User User { get; }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Add(Log model);

        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <param name="tableType">表类型</param>
        /// <param name="type">操作类型</param>
        /// <param name="content">内容</param>
        /// <param name="objectId">主键ID</param>
        /// <returns></returns>
        bool AddSystem(ActionType type, string content, object objectId = null);

        /// <summary>
        /// 添加业务日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        bool AddSystem(ActionType type, object objectId = null);

        /// <summary>
        /// 添加业务日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool AddSystem<T>(ActionType type, IEnumerable<T> ids = null);

        LayuiPaginationOut GetList(LayuiPaginationIn p);

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="value"></param>
        ILogService Append(string value);

        /// <summary>
        /// 拼接新增内容
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        ILogService AppendAdd(string filed, string value);

        /// <summary>
        /// 拼接字符串 带换行
        /// </summary>
        ILogService AppendLine();

        /// <summary>
        /// 拼接字符串 带format
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        ILogService AppendFormat(string format, params string[] args);

        /// <summary>
        /// 拼接修改内容
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        ILogService AppendUpdate(string filed, string oldValue, string newValue);

        /// <summary>
        /// 拼接删除内容
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="result">操作结果</param>
        /// <param name="field">字段描述</param>
        /// <param name="values">值</param>
        ILogService AppendDelete<T>(string result, string field, IEnumerable<T> values);

        /// <summary>
        /// 获得拼接内容
        /// </summary>
        /// <returns></returns>
        string GetContent();
    }

    public class LogService : ILogService
    {
        readonly ILogRepository logRep;
        readonly IUserRepository userRep;
        readonly StringBuilder sb;
        bool updateFirst = true;
        bool addFirst = true;
        public LogService(ILogRepository ilr, IUserRepository iur)
        {
            logRep = ilr;
            userRep = iur;
            this.Type = TableType.NONE;
            sb = new StringBuilder(100);
        }

        public TableType Type { get; set; }

        public string Title { get; set; }

        public User User { get { return this.GetCurrent(); } }

        #region 拼接日志内容

        public ILogService Append(string value)
        {
            sb.Append(value);
            return this;
        }

        public ILogService AppendLine()
        {
            sb.AppendLine();
            return this;
        }

        public ILogService AppendFormat(string format, params string[] args)
        {
            sb.AppendFormat(format, args);
            return this;
        }

        public ILogService AppendAdd(string filed, string value)
        {
            if (!addFirst) sb.AppendLine();
            sb.AppendFormat("{0}为{1}", filed, value);
            if (addFirst) addFirst = false;
            return this;
        }

        public ILogService AppendUpdate(string filed, string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return this;
            //if (!first) sb.Append(",");
            if (!updateFirst) sb.AppendLine();
            sb.AppendFormat("[{0}]由[{1}]-->修改为[{2}]", filed, oldValue, newValue);
            if (updateFirst) updateFirst = false;
            return this;
        }

        public ILogService AppendDelete<T>(string result, string field, IEnumerable<T> values)
        {
            sb.AppendFormat("[{0}],其中[{1}]包括[{0}]", result, field, string.Join(",", values));
            return this;
        }

        public string GetContent()
        {
            updateFirst = true;
            addFirst = true;
            var result = sb.ToString();
            sb.Clear();
            return result;
        }
        #endregion 

        public bool AddSystem(ActionType type, string content, object objectId = null)
        {
            Log model = new Log();
            model.LOGTYPE = (int)LOGType.SYSTEM;
            model.ACTIONTYPE = (int)type;
            model.TITLE = this.Title;
            model.TABLETYPE = (int)this.Type;
            model.CONTENT = content;
            if (objectId != null)
                model.OBJECTID = objectId.ToString();
            return this.Add(model);
        }

        public bool AddSystem(ActionType type, object objectId = null)
        {
            return this.AddSystem(type, this.GetContent(), objectId);
        }

        public bool AddSystem<T>(ActionType type, IEnumerable<T> ids = null)
        {
            return this.AddSystem(type, this.GetContent(), string.Join(",", ids));
        }

        public bool Add(Log model)
        {
            var request = HttpContext.Current.Request;
            var browser = request.Browser;
            model.BROWSER = string.Format("{0}_{1}", browser.Browser, browser.Version);
            model.IPADDRESS = UtilityHepler.GetIPAddress();
            model.URL = request.Url.ToString();
            model.ENABLED = true;

            StringBuilder sb = new StringBuilder();
            if (request.QueryString != null && request.QueryString.Count > 0)
            {
                sb.Append("GET参数");
                sb.Append("{");
                foreach (string item in request.QueryString.Keys)
                {
                    sb.AppendFormat(" \"{0}\":\"{1}\",", item, request.QueryString[item]);
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            if (request.Form != null && request.Form.Count > 0)
            {
                sb.Append("POST参数");
                sb.Append("{");
                foreach (string item in request.Form.Keys)
                {
                    sb.AppendFormat(" \"{0}\":\"{1}\",", item, request.Form[item]);
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("}");
            }
            model.PCONTENT = sb.ToString();
            model.CREATEDATE = DateTime.Now;
            var user = GetCurrent();
            model.CREATEBY = user.ACCOUNT;
            model.CREATENAME = user.GetDisplayName();
            return logRep.Add(model);
        }

        private User GetCurrent()
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

        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            return logRep.GetList(p);
        }
    }
}

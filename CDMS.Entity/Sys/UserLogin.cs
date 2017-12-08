using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_USERLOGIN")]
    public class UserLogin
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public long ID { get; set; }
        /// <summary>
        /// 登录KEY GUID类型
        /// </summary>
        public string LOGINKEY { get; set; }
        /// <summary>
        ///员工编号
        /// </summary>
        public string EMPLOYEEID { get; set; }
        /// <summary>
        ///登录时间
        /// </summary>
        public DateTime LOGINTIME { get; set; }
        /// <summary>
        ///过期时间
        /// </summary>
        public DateTime EXPIREDTIME { get; set; }
        /// <summary>
        ///退出时间
        /// </summary>
        public DateTime? LOGOUTTIME { get; set; }
        /// <summary>
        ///状态 0=退出 1=在线
        /// </summary>
        public bool STATUS { get; set; }
        /// <summary>
        ///登录地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        ///客户端浏览器
        /// </summary>
        public string BROWSER { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string DESCRIPTION { get; set; }
    }
}

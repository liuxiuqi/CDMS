using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_LOG")]
    public class Log
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public long ID { get; set; }
        /// <summary>
        ///日志类型 100=系统日志 1=业务数据 
        /// </summary>
        public int LOGTYPE { get; set; }
        /// <summary>
        ///表名 
        /// </summary>
        public int TABLETYPE { get; set; }
        /// <summary>
        ///操作类型
        /// </summary>
        public int ACTIONTYPE { get; set; }
        /// <summary>
        ///对象主键ID
        /// </summary>
        public string OBJECTID { get; set; }
        /// <summary>
        ///标题
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        ///内容
        /// </summary>
        public string CONTENT { get; set; }
        /// <summary>
        ///参数内容
        /// </summary>
        public string PCONTENT { get; set; }
        /// <summary>
        ///IP地址
        /// </summary>
        public string IPADDRESS { get; set; }
        /// <summary>
        ///路径
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        ///浏览器
        /// </summary>
        public string BROWSER { get; set; }
        /// <summary>
        ///创建人
        /// </summary>
        public string CREATEBY { get; set; }
        /// <summary>
        ///创建人名称
        /// </summary>
        public string CREATENAME { get; set; }
        /// <summary>
        ///创建日志
        /// </summary>
        public DateTime CREATEDATE { get; set; }
        /// <summary>
        ///是否可用
        /// </summary>
        public bool ENABLED { get; set; }
    }
}

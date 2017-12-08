using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    /// <summary>
    /// 机构组织表
    /// </summary>
    [Table("SYS_ORGANIZATION")]
    public class Org
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public string ID { get; set; }
        /// <summary>
        ///CODE
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///NAME
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        ///PARENTID
        /// </summary>
        public string PARENTID { get; set; }
        /// <summary>
        ///INNERPHONE
        /// </summary>
        public string INNERPHONE { get; set; }
        /// <summary>
        ///OUTERPHONE
        /// </summary>
        public string OUTERPHONE { get; set; }
        /// <summary>
        ///MANAGER
        /// </summary>
        public string MANAGER { get; set; }
        /// <summary>
        ///ASSISTANTMANAGER
        /// </summary>
        public string ASSISTANTMANAGER { get; set; }
        /// <summary>
        ///FAX
        /// </summary>
        public string FAX { get; set; }
        /// <summary>
        ///ZIPCODE
        /// </summary>
        public string ZIPCODE { get; set; }
        /// <summary>
        ///ADDRESS
        /// </summary>
        public string ADDRESS { get; set; }
        /// <summary>
        ///REMARK
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        ///SORTID
        /// </summary>
        public int SORTID { get; set; }
        /// <summary>
        ///CREATEBY
        /// </summary>
        public string CREATEBY { get; set; }
        /// <summary>
        ///CREATEDATE
        /// </summary>
        public DateTime CREATEDATE { get; set; }
        /// <summary>
        ///UPDATEBY
        /// </summary>
        public string UPDATEBY { get; set; }
        /// <summary>
        ///UPDATEDATE
        /// </summary>
        public DateTime UPDATEDATE { get; set; }
        /// <summary>
        ///ENABLED
        /// </summary>
        public bool ENABLED { get; set; }
    }
}

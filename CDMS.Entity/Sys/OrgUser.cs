using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    /// <summary>
    /// 组织用户表
    /// </summary>
    [Table("SYS_ORGANIZATIONUSER")]
    public class OrgUser
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }
        /// <summary>
        ///组织ID
        /// </summary>
        public string ORGID { get; set; }
        /// <summary>
        ///用户ID
        /// </summary>
        public int USERID { get; set; }
    }
}

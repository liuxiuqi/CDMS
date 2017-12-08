using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    /// <summary>
    /// 角色用户表
    /// </summary>
    [Table("SYS_ROLEUSER")]
    public class RoleUser
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int ROLEID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int USERID { get; set; }
    }
}

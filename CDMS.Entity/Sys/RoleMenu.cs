using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Table("SYS_ROLEMENU")]
    public class RoleMenu
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
        /// 菜单ID
        /// </summary>
        public int MENUID { get; set; }
    }
}

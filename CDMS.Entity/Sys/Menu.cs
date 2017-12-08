using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_MENU")]
    public class Menu
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        ///TITLE
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        ///代码
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///图片
        /// </summary>
        public string IMG { get; set; }
        /// <summary>
        ///类型 1=目录 2=菜单 3=按钮
        /// </summary>
        public int TYPE { get; set; }
        /// <summary>
        ///URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        ///目标指向
        /// </summary>
        public string TARGET { get; set; }
        /// <summary>
        ///父ID
        /// </summary>
        public int PARENTID { get; set; }
        /// <summary>
        ///样式名称
        /// </summary>
        public string CLASSNAME { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        ///排序ID
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

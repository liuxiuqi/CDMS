using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_CATEGORY")]
    public class Category
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }
        /// <summary>
        ///TYPE
        /// </summary>
        public int TYPE { get; set; }
        /// <summary>
        ///NAME
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        ///CODE
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///PARENTID
        /// </summary>
        public int PARENTID { get; set; }
        /// <summary>
        ///REMARK
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        ///ISNAV
        /// </summary>
        public bool ISNAV { get; set; }
        /// <summary>
        ///ISSPECIAL
        /// </summary>
        public bool ISSPECIAL { get; set; }
        /// <summary>
        ///URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        ///TARGET
        /// </summary>
        public int TARGET { get; set; }
        /// <summary>
        /// SORTID
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

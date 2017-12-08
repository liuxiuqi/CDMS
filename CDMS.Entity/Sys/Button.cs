using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_BUTTON")]
    public class Button
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }
        /// <summary>
        ///NAME
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        ///TITLE
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        ///IMG
        /// </summary>
        public string IMG { get; set; }
        /// <summary>
        ///CLASSNAME
        /// </summary>
        public string CLASSNAME { get; set; }
        /// <summary>
        ///CODE
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///SORTID
        /// </summary>
        public int SORTID { get; set; }
        /// <summary>
        ///TYPE
        /// </summary>
        public string TYPE { get; set; }
        /// <summary>
        ///REMARK
        /// </summary>
        public string REMARK { get; set; }
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

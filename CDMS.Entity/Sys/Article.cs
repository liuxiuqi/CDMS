using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_ARTICLE")]
    public class Article
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public long ID { get; set; }
        /// <summary>
        ///TITLE
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        ///SUBTITLE
        /// </summary>
        public string SUBTITLE { get; set; }
        /// <summary>
        ///SUMMARY
        /// </summary>
        public string SUMMARY { get; set; }
        /// <summary>
        ///AUTHOR
        /// </summary>
        public string AUTHOR { get; set; }
        /// <summary>
        ///CONTENTS
        /// </summary>
        public string CONTENTS { get; set; }
        /// <summary>
        ///CATETORYID
        /// </summary>
        public int CATEGORYID { get; set; }
        /// <summary>
        ///CODE
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///STATUS
        /// </summary>
        public int STATUS { get; set; }
        /// <summary>
        ///ISRED
        /// </summary>
        public bool ISRED { get; set; }
        /// <summary>
        ///ISHOT
        /// </summary>
        public bool ISHOT { get; set; }
        /// <summary>
        ///ISTOP
        /// </summary>
        public bool ISTOP { get; set; }
        /// <summary>
        ///ISDISPLAY
        /// </summary>
        public bool ISDISPLAY { get; set; }
        /// <summary>
        ///CLICK
        /// </summary>
        public int CLICK { get; set; }
        /// <summary>
        ///LINKURL
        /// </summary>
        public string LINKURL { get; set; }
        /// <summary>
        ///SEOTITLE
        /// </summary>
        public string SEOTITLE { get; set; }
        /// <summary>
        ///SEOKEYWORDS
        /// </summary>
        public string SEOKEYWORDS { get; set; }
        /// <summary>
        ///SEOREMARK
        /// </summary>
        public string SEOREMARK { get; set; }
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

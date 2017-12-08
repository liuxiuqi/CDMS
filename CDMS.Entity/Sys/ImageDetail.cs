using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_IMAGEDETAIL")]
    public class ImageDetail
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }
        /// <summary>
        ///IMAGEID
        /// </summary>
        public int IMAGEID { get; set; }
        /// <summary>
        ///TITLE
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        ///ALT
        /// </summary>
        public string ALT { get; set; }
        /// <summary>
        ///TYPE
        /// </summary>
        public int TYPE { get; set; }
        /// <summary>
        ///URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        ///ISLOCAL
        /// </summary>
        public bool ISLOCAL { get; set; }
        /// <summary>
        ///WIDTH
        /// </summary>
        public int WIDTH { get; set; }
        /// <summary>
        ///HEIGHT
        /// </summary>
        public int HEIGHT { get; set; }
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

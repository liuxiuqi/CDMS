using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Table("SYS_ROLE")]
    public class Role
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

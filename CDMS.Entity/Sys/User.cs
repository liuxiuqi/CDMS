using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    [Serializable]
    [Table("SYS_USER")]
    public class User
    {
        /// <summary>
        ///ID
        /// </summary>
        [Column(true)]
        public int ID { get; set; }
        /// <summary>
        ///ACCOUNT
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        ///PWD
        /// </summary>
        public string PWD { get; set; }
        /// <summary>
        ///CNNAME
        /// </summary>
        public string CNNAME { get; set; }
        /// <summary>
        ///ENNAME
        /// </summary>
        public string ENNAME { get; set; }
        /// <summary>
        ///SEX
        /// </summary>
        public string SEX { get; set; }
        /// <summary>
        ///AGE
        /// </summary>
        public int AGE { get; set; }
        /// <summary>
        ///EMAIL
        /// </summary>
        public string EMAIL { get; set; }

        public string PHONE { get; set; }
        /// <summary>
        ///THEME
        /// </summary>
        public string THEME { get; set; }
        /// <summary>
        ///QUESTION
        /// </summary>
        public string QUESTION { get; set; }
        /// <summary>
        ///ANSWER
        /// </summary>
        public string ANSWER { get; set; }
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
        ///STATUS
        /// </summary>
        public int STATUS { get; set; }
        /// <summary>
        ///IMG
        /// </summary>
        public string IMG { get; set; }

        public string GetDisplayName()
        {
            string name = this.CNNAME;
            if (string.IsNullOrEmpty(name))
            {
                name = this.ENNAME;
                if (string.IsNullOrEmpty(name))
                    name = this.ACCOUNT;
            }
            return name;
        }

        public string GetUserFaceUrl()
        {
            string url = this.IMG;
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath(url);
                    bool flag = System.IO.File.Exists(path);
                    if (flag) return url;
                }
                catch { }
            }
            return "~/Resources/images/0.jpg";
        }
    }
}

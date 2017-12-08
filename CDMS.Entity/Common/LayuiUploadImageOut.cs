using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    public class LayuiUploadImageOut
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }

        public LayuiUploadImageOut() { }

        public LayuiUploadImageOut(int code, string msg, string src, string title = "")
        {
            this.code = code;
            this.msg = msg;
            this.data = new { src = src, title = title };
        }

        public LayuiUploadImageOut(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }
    }
}

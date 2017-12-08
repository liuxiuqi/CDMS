using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    public class LayuiPaginationOut
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public object data { get; set; }
        public string costtime { get; set; }

        public LayuiPaginationOut() { }

        public LayuiPaginationOut(PaginationIn p, object data)
        {
            this.code = 0;
            this.count = p.total;
            this.costtime = p.costtime;
            this.msg = "";
            this.data = data;
        }
        public LayuiPaginationOut(LayuiPaginationIn p, object data)
        {
            this.code = 0;
            this.count = p.total;
            this.costtime = "";
            this.msg = "";
            this.data = data;
        }

        public LayuiPaginationOut(IEnumerable<object> data)
        {
            this.code = 0;
            if (data != null)
                this.count = data.Count();
            this.msg = "";
            this.data = data;
        }

        public LayuiPaginationOut(int total, IEnumerable<object> data)
        {
            this.code = 0;
            this.count = total;
            this.msg = "";
            this.data = data;
        }
    }
}

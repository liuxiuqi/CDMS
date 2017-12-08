using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    public class LayuiPaginationIn
    {
        public int page { get; set; }

        public int limit { get; set; }

        public int total { get; set; }

        public string json { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    public class ImageVM
    {
        public Image Image { get; set; }

        public IEnumerable<ImageDetail> Details { get; set; }
    }
}

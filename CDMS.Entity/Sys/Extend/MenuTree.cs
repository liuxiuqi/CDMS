using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDMS.Entity
{
    public class MenuTree
    {
        public int id { get; set; }

        public string title { get; set; }

        public string icon { get; set; }

        public bool spread { get; set; }

        public string url { get; set; }

        public IEnumerable<MenuTree> children { get; set; }

        public MenuTree()
        {
        }

        public MenuTree(int id, string title, string icon, string url)
        {
            this.id = id;
            this.title = title;
            this.icon = icon;
            this.url = url;
        }

        public MenuTree(Menu m)
        {
            this.id = m.ID;
            this.title = m.NAME;
            this.icon = m.IMG;
            this.url = m.URL;
            this.spread = true;
        }
    }
}

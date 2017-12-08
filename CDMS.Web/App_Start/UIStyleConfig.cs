using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMS.Utility;
using CDMS.Entity;

namespace CDMS.Web
{
    public class UIStyleConfig
    {
        public static string GetCurrentUIName()
        {
            string name = CookieHelper.Get(WebConst.UserLoginUICookieKey);
            if (string.IsNullOrEmpty(name)) name = "default";
            return name;
        }
    }
}
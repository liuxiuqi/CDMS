using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CDMS.Utility
{
    public class JsonHelper
    {
        public static string ToJson(object o, string format = "")
        {
            JsonSerializerSettings jss = new JsonSerializerSettings();
            if (string.IsNullOrEmpty(format)) format = "yyyy-MM-dd HH:mm:ss";
            jss.DateFormatString = format;
            return JsonConvert.SerializeObject(o, jss);
        }

        public static object ToObject(string s)
        {
            return JsonConvert.DeserializeObject(s);
        }

        public static T ToObject<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using CDMS.Entity;
using CDMS.Utility;

namespace CDMS.Service
{
    public interface IDevelopLogService : IDependency
    {
        LayuiPaginationOut GetList(LayuiPaginationIn p);

        AjaxResult DeleteFiles(string[] fileNames);

        string GetFileContent(string path);
    }

    public class DevelopLogService : IDevelopLogService
    {
        private string basePath = "~/Log";

        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            var list = GetList(basePath);
            string json = p.json;
            int total = 0;
            IEnumerable<DevelopLog> logs = list;
            if (!string.IsNullOrEmpty(json))
            {
                DateTime begin = json.ToDateTime(DateTime.MinValue);
                if (begin > DateTime.MinValue)
                {
                    DateTime end = begin.AddDays(1);
                    logs = logs.Where(m => m.LastUpdteTime >= begin && m.LastUpdteTime < end);
                }
            }
            if (logs != null) total = logs.Count();

            int skip = (p.page - 1) * p.limit;
            logs = logs.Skip(skip).Take(p.limit);
            if (logs != null) logs = logs.OrderByDescending(m => m.LastUpdteTime);
            return new LayuiPaginationOut(total, logs);
        }

        public string GetFileContent(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            if (File.Exists(path))
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }
            return string.Empty;
        }

        public AjaxResult DeleteFiles(string[] fileNames)
        {
            if (fileNames == null || fileNames.Length < 1)
                return new AjaxResult(false, "没有找到任何日志文件");

            try
            {
                foreach (var item in fileNames)
                {
                    File.Delete(item);
                }
                return new AjaxResult(true, "日志文件删除成功");
            }
            catch (Exception e)
            {
                return new AjaxResult(false, "日志文件删除失败--" + e.Message);
            }
        }

        private List<DevelopLog> GetList(string path)
        {
            List<DevelopLog> list = new List<DevelopLog>();
            string mapPath = MapPath(path);
            var children = Directory.GetFiles(mapPath, "*", SearchOption.AllDirectories);
            if (children != null && children.Length > 0)
            {
                foreach (var item in children)
                {
                    list.Add(new DevelopLog(new FileInfo(item)));
                }
            }
            return list;
        }

        private string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CDMS.Entity
{
    public class DevelopLog
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdteTime { get; set; }

        public string LastUpdateTimeString
        {
            get
            {
                return this.LastUpdteTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 大小
        /// </summary>
        public double Size { get; set; }

        public DevelopLog() { }

        public DevelopLog(FileInfo file)
        {
            if (file != null)
            {
                this.Name = file.Name;
                this.Path = file.FullName;
                this.LastUpdteTime = file.LastWriteTime;
                this.Size = Math.Round((file.Length / 1024f), 1);
            }
        }
    }
}

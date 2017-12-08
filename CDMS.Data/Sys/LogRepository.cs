using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDMS.Entity;
using CDMS.Utility;
using Roc.Data;

namespace CDMS.Data
{
    public interface ILogRepository : IRepository<Log>
    {
        LayuiPaginationOut GetList(LayuiPaginationIn p);
    }

    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository()
        {

        }

        public LayuiPaginationOut GetList(LayuiPaginationIn p)
        {
            sql.SelectAll();
            var log = JsonHelper.ToObject<Log>(p.json);
            sql.And(m => m.ENABLED == true);
            sql.And(m => m.LOGTYPE == log.LOGTYPE);
            if (log.ACTIONTYPE > 0)
            {
                sql.And(m => m.ACTIONTYPE == log.ACTIONTYPE);
            }
            if (log.CREATEDATE > DateTime.MinValue)
            {
                DateTime begin = log.CREATEDATE;
                DateTime end = begin.AddDays(1);
                sql.And(m => m.CREATEDATE >= begin && m.CREATEDATE <= end);
            }
            sql.OrderByDescending(m => m.ID);
            var list = base.GetPageList(p);
            return new LayuiPaginationOut(p, list);
        }
    }
}

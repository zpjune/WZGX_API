using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu
{
    public class BBTJCXModule
    {
        BBTJCXDB db = new BBTJCXDB();
        public Dictionary<string,object> GetFCLInfo(string Date,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetFCLInfo(Date);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    //r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功但是没有数据";
                    //r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = 0;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class GCWHModule
    {
        GCWHDB db = new GCWHDB();

        public Dictionary<string,object> GetGCInfo(string DW_CODE,string DW_NAME,string DW_ISSS,int limit,int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetGCInfo(DW_CODE, DW_NAME,DW_ISSS);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["message"] = "成功！";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功！,但是没有数据";
                    r["items"] = new DataTable();
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }


        public Dictionary<string,object> CreateGCInfo(Dictionary<string,string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateGCInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }


        public Dictionary<string, object> DelGCInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DelGCInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> EditGCInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.EditGCInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

    }
}

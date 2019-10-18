using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class KCDDWHModule
    {
        KCDDWHDB db = new KCDDWHDB();

        public Dictionary<string, object> GetKCDDInfo(string KCDD_CODE, string KCDD_NAME, int limit, int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetKCDDInfo(KCDD_CODE, KCDD_NAME);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> GetDWInfo()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetDWInfo();
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }


        public Dictionary<string, object> CreateKCDDInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateKCDDInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "success";
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


        public Dictionary<string, object> DelKCDDInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DelKCDDInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "success";
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

        public Dictionary<string, object> EditKCDDInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.EditKCDDInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "success";
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

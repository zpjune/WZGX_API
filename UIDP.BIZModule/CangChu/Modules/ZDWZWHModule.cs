using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class ZDWZWHModule
    {
        ZDWZWHDB db = new ZDWZWHDB();

        public Dictionary<string,object> GetZDWZWHInfo(string WL_LOCATIONCODE, string WL_CODE,int limit,int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetZDWZWHInfo(WL_LOCATIONCODE, WL_CODE);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "success";
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                    r["total"] = 0;
                }
            }
            catch(Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }

        public Dictionary<string,object> CreateZDWZWHInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateZDWZWHInfo(d);
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
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public object GetPMCODE()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> EditZDWZWHInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.EditZDWZWHInfo(d);
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


        public Dictionary<string, object> DelZDWZWHInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DelZDWZWHInfo(d);
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

        public Dictionary<string, object> GetKCDDInfo()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetKCDDInfo();
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "success";
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["message"] = e.Message;
                r["code"] = -1;
            }
            return r;
        }
    }
}

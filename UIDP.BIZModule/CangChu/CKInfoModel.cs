using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu
{
    public class CKInfoModel
    {
        CKInfoDB db = new CKInfoDB();
        public Dictionary<string, object> GetRKInfo(int page, int limit, string CKTime, string LocationNumber)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetCKInfo(CKTime, LocationNumber);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but have no data";
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = "failed!" + e.Message;
            }
            return r;
        }

        public Dictionary<string, object> CreateCKInfo(Dictionary<string, object> d)
        {
            d["CK_ID"] = Guid.NewGuid();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateCKInfo(d);
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

        public Dictionary<string, object> UpdateCKInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.UpdateCKInfo(d);
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

        public Dictionary<string, object> DeleteCKInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DeleteCKInfo(d);
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

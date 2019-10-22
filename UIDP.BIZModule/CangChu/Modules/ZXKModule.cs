using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class ZXKModule
    {
        ZXKDB db = new ZXKDB();
        public Dictionary<string,object> GetDRKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.GetDRKInfo(MATNR, info, FacCode,page, limit);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["total"] = ds.Tables[1].Rows[0]["TOTAL"];
                    r["items"] = ds.Tables[0];
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但是没有数据";
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


        public Dictionary<string, object> GetDCKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.GetDCKInfo(MATNR, info, FacCode,page, limit);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["total"] = ds.Tables[1].Rows[0]["TOTAL"];
                    r["items"] = ds.Tables[0];
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功,但是没有数据";
                    r["total"] = 0;

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

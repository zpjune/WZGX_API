using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class SWKCModle
    {
        SWKCDB db = new SWKCDB();
        public Dictionary<string,object> GetFacInfo(string WERKS, string LGORT, string LGORT_NAME,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetFacInfo(WERKS, LGORT, LGORT_NAME);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "success";
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "success,but no info";
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string,object> GetCompositeInfo(string WERKS, string LGORT, string LGORT_NAME, string MATNR, string MAKTX,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetCompositeInfo(WERKS, LGORT, LGORT_NAME, MATNR, MAKTX);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["message"] = "success";
                    r["totoal"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2001;
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

        //public Dictionary<string,object> GetParentList(string MATKL,int page,int limit,int level)
        //{
        //    Dictionary<string, object> r = new Dictionary<string, object>();
        //    try
        //    {
        //        DataTable dt = db.GetAllInfo(MATKL);
        //    }
        //}

        //public List<>
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    /// <summary>
    /// 总库存页面逻辑
    /// </summary>
   public class TotalModule
    {
       TotalDB db = new TotalDB();

        /// <summary>
        /// 查询总库存库存资金-饼图 港东C27C  港西 C27D 油区C27G  港狮C279 港华C27B
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetKCZJ()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                
                DataTable dt = db.GetKCZJ();
                if (dt.Rows.Count > 0)
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d["TOTAL"] = dt.Select("WERKS='TOTAL'")[0][0];
                    d["C27C"] = dt.Select("WERKS='C27C'")[0][0];
                    d["C27D"] = dt.Select("WERKS='C27D'")[0][0];
                    d["C27G"] = dt.Select("WERKS='C27G'")[0][0];
                    d["C279"] = dt.Select("WERKS='C279'")[0][0];
                    d["C27B"] = dt.Select("WERKS='C27B'")[0][0];
                    r["code"] = 2000;
                    r["items"] = d;//dt
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
        /// <summary>
        /// 查询实物库存-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public Dictionary<string, object> GetSWKC(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataTable dt = db.GetSWKC(WERKS_NAME,  LGORTNAME,  MATNR,  MATKL);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));//dt
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                    r["items"] = new DataTable();//dt
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
        /// <summary>
        /// 查询积压物资-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public Dictionary<string, object> GetJYWZ(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataTable dt = db.GetJYWZ(WERKS_NAME, LGORTNAME, MATNR, MATKL);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));//dt
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                    r["items"] = new DataTable();//dt
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
        public Dictionary<string, object> GetCRKJE(string year)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataSet ds = db.GetCRKJE(year);
                if (ds.Tables.Count > 0)
                {
                    List<DataTable> list = new List<DataTable>();
                    list.Add(ds.Tables["CKJE"]);
                    list.Add(ds.Tables["RKJE"]);
                    r["code"] = 2000;
                    r["items"] = list;//dt
                    r["message"] = "success";
                    r["total"] = 0;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                    r["items"] = new DataTable();//dt
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

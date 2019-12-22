using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

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
                DataSet ds = new DataSet();
                if (FacCode == "09")
                {
                    CXZGKDB db1 = new CXZGKDB();
                    ds = db1.GetDRKInfo(MATNR, info, FacCode, page, limit);
                }
                else
                {
                    ds = db.GetDRKInfo(MATNR, info, FacCode, page, limit);
                }               
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
                    r["items"] = new DataTable();
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
                DataSet ds = new DataSet();
                if (FacCode == "09")
                {
                    CXZGKDB db1 = new CXZGKDB();
                    ds = db1.GetDCKInfo(MATNR, info, FacCode, page, limit);
                }
                else
                {
                    ds = db.GetDCKInfo(MATNR, info, FacCode, page, limit);
                }
                //DataSet ds = db.GetDCKInfo(MATNR, info, FacCode,page, limit);
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
                    r["items"] = new DataTable();
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
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE">大库编码</param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetFK_JYWZ(string ISWZ, string WERKS, string DKCODE, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = new DataTable();
                if (DKCODE == "09")
                {
                    CXZGKDB db1 = new CXZGKDB();
                    dt=db1.GetFK_JYWZ(DKCODE, MATNR, MATKL);
                }
                else
                {
                    dt = db.GetFK_JYWZ( ISWZ,  WERKS, DKCODE, MATNR, MATKL);
                }
                //DataTable dt = db.GetFK_JYWZ(DKCODE, MATNR, MATKL);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));//dt
                    r["message"] = "成功！";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功！但是没有数据";
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
        /// 查询平面图内各库状态
        /// </summary>
        /// <param name="FacCode">工厂代码</param>
        /// <returns></returns>
        /***
         * 本段代码逻辑如下：
         * 1.根据查询SQL获取最大仓位号
         * 2.根据最大仓位号循环读取dt中的相关数据，采用二进制字符串的方式来表示状态是否存在，1表示存在，0表示不存在
         * 3.物资共有3中状态，所以在第一个循环当中还需要加第二个循环读取判断其他两个状态是否存在
         * 4.由于仓位号是字符串，所以对循环数i需要进行处理，i<10时将其变为01,02等
         ***/ 
        public Dictionary<string, object> GetFacStatus(string FacCode)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = new DataTable();
                if (FacCode == "09")
                {
                    CXZGKDB db1 = new CXZGKDB();
                    dt=db1.GetFacStatus(FacCode);
                }
                else
                {
                    dt = db.GetFacStatus(FacCode);
                }
                //DataTable dt = db.GetFacStatus(FacCode);
                int MaxFacCode;
                int.TryParse(dt.Rows[dt.Rows.Count-1]["LG"].ToString(), out MaxFacCode);
                string StatusStr = string.Empty;
                for (int i = 1; i <= MaxFacCode; i++)
                {
                    string Stri = i.ToString();
                    while (Stri.Length < 2)
                    {
                        Stri = "0" + Stri;
                    }
                    for (int j = 1; j < 4; j++)
                    {
                        if (dt.Select("LG='" + Stri + "' AND Status=" + j).Length > 0)
                        {
                            StatusStr += "1";
                        }
                        else
                        {
                            StatusStr += "0";
                        }
                    }
                }
                r["code"] = 2000;
                r["message"] = "成功！";
                r["items"] = StatusStr;
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 重点物资储备查询-分库（停用）
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public Dictionary<string, object> getZDWZCB(string DKCODE, string WERKS_NAME, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = new DataTable();
                if (DKCODE == "09")
                {
                    CXZGKDB db1 = new CXZGKDB();
                    dt=db1.getZDWZCB(DKCODE, WERKS_NAME, MATNR, MATKL);
                }
                else
                {
                    dt = db.getZDWZCB(DKCODE, WERKS_NAME, MATNR, MATKL);
                }

                //DataTable dt = db.getZDWZCB(DKCODE,WERKS_NAME, MATNR, MATKL);
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
        /// 重点物资储备查询-分库
        /// </summary>
        /// <param name="DKCODE"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Dictionary<string, object> getDetailZDWZCBTOTAL(string DKCODE,  string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = new DataTable();
                dt = db.getDetailZDWZCBTOTAL(DKCODE,  MATNR, MATKL);
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
        /// 重点物资储备查询-分库明细
        /// </summary>
        /// <param name="WERKS"></param>
        /// <param name="DKCODE"></param>
        /// <param name="WERKS_NAME"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Dictionary<string, object> getDetailZDWZCBTOTALDETAIL(string WERKS,string DKCODE, string WERKS_NAME, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = new DataTable();
                dt = db.getDetailZDWZCBTOTALDETAIL(WERKS,DKCODE, WERKS_NAME, MATNR, MATKL);
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
        /// 重点物资出入库查询-分库
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public Dictionary<string, object> getZDWZCRK(string DKCODE, string year, string MATNR)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                Result res = new Result();
                DataSet ds = db.getZDWZCRK(DKCODE,year, MATNR);
                if (ds.Tables.Count > 0)
                {
                    res.ZGCB = ds.Tables["zgcb"];
                    res.KC = ds.Tables["kc"];
                    res.RK = ds.Tables["rk"];
                    res.CK = ds.Tables["ck"];
                    r["code"] = 2000;
                    r["items"] = res;
                    r["message"] = "success";
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                    r["items"] = new DataTable();//dt
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
        /// 重点物资出入库明细-去向明细 分库
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MONTH"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Dictionary<string, object> getZDWZCRKDetail(string DKCODE, string MATNR, string MONTH, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = new DataTable();
                if (DKCODE == "09")
                {
                    CXZGKDB db1 = new CXZGKDB();
                    dt = db1.getZDWZCRKDetail(DKCODE, MATNR, MONTH);
                }
                else
                {
                    dt = db.getZDWZCRKDetail(DKCODE, MATNR, MONTH);
                }
                //DataTable dt = db.getZDWZCRKDetail(DKCODE,MATNR, MONTH);
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

        public Dictionary<string,object> GetStatusDetail(string LGPLA,string MATNR, string WERKS,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetStatusDetail(LGPLA,MATNR,WERKS);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功,但是没有数据";
                    r["items"] = dt;
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

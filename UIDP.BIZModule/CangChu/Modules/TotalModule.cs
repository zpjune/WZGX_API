﻿using System;
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
        /// <summary>
        /// 总库存-查询输入库统计金额
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 总库存-查询出库如明细金额
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Dictionary<string, object> getCRKDetail(string year, string month)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataSet ds = db.getCRKDetail(year,month);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtNew = new DataTable();
                    dtNew.Columns.Add("DKName");
                    dtNew.Columns.Add("RKJE");
                    dtNew.Columns.Add("RKL");
                    dtNew.Columns.Add("CKJE");
                    dtNew.Columns.Add("CKL");
                    foreach (DataRow rowDKName in ds.Tables["DK_NAME"].Rows)
                    {
                        int i = 0;//判断下面是否有数据，有才会add到dtnew里
                        DataRow newRow = dtNew.NewRow();
                        DataRow[] rowRK = ds.Tables["RKJE_Detail"].Select("CKH_NAME='"+rowDKName["CKH_NAME"] +"'");
                        if (rowRK.Length>0) {
                            i = 1;
                            newRow["DKName"] = rowRK[0]["CKH_NAME"];
                            newRow["RKJE"] = rowRK[0]["RKJE"];
                            newRow["RKL"] = rowRK[0]["RKL"];
                        }
                        rowRK = ds.Tables["CKJE_Detail"].Select("CKH_NAME='" + rowDKName["CKH_NAME"] + "'");
                        if (rowRK.Length > 0)
                        {
                            i = 1;
                            newRow["DKName"] = rowRK[0]["CKH_NAME"];
                            newRow["CKJE"] = rowRK[0]["CKJE"];
                            newRow["CKL"] = rowRK[0]["CKL"];
                        }
                        if(i==1){
                            dtNew.Rows.Add(newRow);
                        }
                    }
                    r["code"] = 2000;
                    r["items"] = dtNew;//dt
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
        /// <summary>
        /// 总库查询保管员工作量
        /// </summary>
        /// <param name="month"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Dictionary<string, object> getBGYGZL(string month,string workerName, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getBGYGZL(month, workerName);
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
        /// 总库存保管员工作量明细查询
        /// </summary>
        /// <param name="nianyue">年月</param>
        /// <param name="TZDType">1 入库单 2 出库单</param>
        /// <param name="workerCode">员工编号</param>
        public Dictionary<string, object> getBGYGZLDetail(string nianyue, string TZDType, string workerCode, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.getBGYGZLDetail(nianyue,  TZDType,  workerCode);
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
        /// 重点物资储备查询-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public Dictionary<string, object> getZDWZCB(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataTable dt = db.getZDWZCB(WERKS_NAME, LGORTNAME, MATNR, MATKL);
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
        /// 重点物资出入库查询-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public Dictionary<string, object> getZDWZCRK(string month,string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataTable dt = db.getZDWZCRK(month,WERKS_NAME, LGORTNAME, MATNR, MATKL);
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
    }
}

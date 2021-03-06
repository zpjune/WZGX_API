﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class CXZGKDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 查询待出库信息
        /// </summary>
        /// <param name="MATNR">物料编码</param>
        /// <param name="info">物料描述</param>
        /// <param name="FacCode">库存代码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        public DataSet GetDRKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {

            string PartSql = " {0} SELECT {1} FROM {2}";
            string MainSql = "（SELECT a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,SUM(a.MENGE) AS MENGE,c.ERNAM,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1,f.DW_NAME FROM ZC10MMDG072 a " +
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " JOIN WZ_KCDD g ON a.WERKS=g.DWCODE AND a.LGORT=g.KCDD_CODE" +
                " WHERE a.ZSTATUS='01' a.ZBL<>'是' " +
                " AND g.CKH NOT IN('01','02','03','04','05','06','07','08')";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND a.MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql += " AND e.MAKTX like'" + info + "%'";
            }

            //string KCDDSql = "SELECT KCDD_CODE,DWCODE FROM WZ_KCDD WHERE CKH NOT IN('01','02','03','04','05','06','07','08')";
            //DataTable KCDDData = db.GetDataTable(KCDDSql);
            //if (KCDDData.Rows.Count > 0)
            //{
            //    MainSql += " AND(";
            //    foreach (DataRow dr in KCDDData.Rows)
            //    {
            //        MainSql += " (a.WERKS='" + dr["DWCODE"] + "'";
            //        MainSql += "  AND a.LGORT='" + dr["KCDD_CODE"] + "')";
            //        if (!dr.Equals(KCDDData.Rows[KCDDData.Rows.Count - 1]))
            //        {
            //            MainSql += " OR";
            //        }
            //    }
            //    MainSql += ")";
            //}
            MainSql += " GROUP BY a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,c.ERNAM,a.WERKS,c.NAME1,f.DW_NAME ORDER BY a.ZDHTZD DESC)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }


        public DataSet GetDCKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {
            string PartSql = " {0} SELECT {1} FROM {2}";
            string MainSql = "（SELECT a.ZCKTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,SUM(a.ZFHSL) AS ZFHSL,c.ERNAM,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1,f.DW_NAME FROM ZC10MMDG078 a " +
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " LEFT JOIN WZ_KCDD g ON a.WERKS=g.DWCODE AND a.LGORT=g.KCDD_CODE" +
                " WHERE a.ZSTATUS='01' a.ZBL<>'是' " +
                " AND g.CKH NOT IN('01','02','03','04','05','06','07','08')";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND a.MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql += " AND e.MAKTX like'" + info + "%'";
            }

            //string KCDDSql = "SELECT KCDD_CODE,DWCODE FROM WZ_KCDD WHERE  CKH NOT IN('01','02','03','04','05','06','07','08')";
            //DataTable KCDDData = db.GetDataTable(KCDDSql);
            //if (KCDDData.Rows.Count > 0)
            //{
            //    MainSql += " AND(";
            //    foreach (DataRow dr in KCDDData.Rows)
            //    {
            //        MainSql += " (a.WERKS='" + dr["DWCODE"] + "'";
            //        MainSql += "  AND a.LGORT='" + dr["KCDD_CODE"] + "')";
            //        if (!dr.Equals(KCDDData.Rows[KCDDData.Rows.Count - 1]))
            //        {
            //            MainSql += " OR";
            //        }
            //    }
            //    MainSql += ")";
            //}
            MainSql += " GROUP BY a.ZCKTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,c.ERNAM,a.WERKS,c.NAME1,f.DW_NAME ORDER BY a.ZCKTZD DESC)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }
        /// <summary>
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE">大库编码</param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public DataTable GetFK_JYWZ(string DLCODE,string DKCODE, string MATNR, string MATKL)
        {
            string sql = @" select sum(GESME) GESME,WERKS,WERKS_NAME,LGORT_NAME,LGORT,MAX(MATKL)MATKL,MAX(MAKTX)MAKTX,ZSTATUS,MAX(MEINS)MEINS,
                            '积压' ZT
                               ,werks,matnr,lgort 
                            from CONVERT_SWKC  ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += "where months_between(sysdate,to_date(ERDAT,'yyyy-mm-dd'))>6 AND substr(LGPLA,1,2) NOT IN('01','02','03','04','05','06','07','08')" +
                " AND SUBSTR(MATKL,0,2)='"+ DLCODE+"'";

            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  MATKL like'%" + MATKL + "%'";
            }
            sql += "group by werks,matnr,lgort,zstatus,WERKS_NAME,LGORT_NAME ";//
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 查询平面图仓位状态
        /// </summary>
        /// <param name="FacCode">大库编码</param>
        /// <param name="Month_between">积压月份上限，默认为6</param>
        /// <returns></returns>
        public DataTable GetFacStatus(string FacCode, int Month_between = 6)
        {
            //查询是否积压sql，后续查询用union all 拼上
            string sql = " SELECT SUBSTR(LGPLA,3,2) AS LG,01 AS Status FROM CONVERT_SWKC a" +
                " where ZSTATUS='04'" +
                " AND SUBSTR(LGPLA,0,2)NOT IN('01','02','03','04','05','06','07','08')" +
                " AND MONTHS_BETWEEN(TO_DATE('" + DateTime.Now.ToString("yyyyMMdd") + "','yyyyMMdd'),TO_DATE(ERDAT,'yyyyMMdd'))>" + Month_between +
                " GROUP BY SUBSTR(LGPLA,3,2)" +
                " ORDER BY SUBSTR(LGPLA,3,2)";
            return db.GetDataTable(sql);
        }
        ///重点物资储备-分库
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCB(string DKCODE, string WERKS_NAME, string MATNR, string MATKL)
        {
            string sql = @" select sum(A.GESME) GESME,A.WERKS,A.WERKS_NAME,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS 
                        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where A.KCTYPE<>3 AND C.CKH NOT IN('01','02','03','04','05','06','07','08')";
            if (!string.IsNullOrEmpty(WERKS_NAME))
            {
                sql += " and  A.WERKS_NAME like'%" + WERKS_NAME + "%'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            sql += "  group by A.WERKS,A.MATNR,A.WERKS_NAME ";//
            return db.GetDataTable(sql);
        }
        ///重点物资出入库查询-分库
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCRK(string DKCODE, string yearmonth, string MATNR)
        {
            string year = yearmonth.Substring(0, 4);
            string _month = yearmonth.Substring(4, 2);
            string sql = @" 
                        select G.*,H.RKSL,I.RKSUMSL,J.CKSL,K.CKSUMSL,F.WL_CODE,'" + yearmonth + "' MONTH ";
            sql += @"           from WZ_ZDWZPZ F
                         LEFT JOIN (
                        select sum(A.GESME) GESME，
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR";
            sql += "           join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS AND A.KCTYPE<>3 AND  C.CKH NOT IN('01','02','03','04','05','06','07','08')";

            sql += @"        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                         group by A.MATNR) G ON F.WL_CODE=G.MATNR
                        LEFT JOIN 
                        (
                        select SUM(A.ZDHSL) RKSL,A.MATNR
                        from ZC10MMDG072 A 
                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "     WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            sql += @"         GROUP BY A.MATNR) H ON F.WL_CODE=H.MATNR

                        LEFT JOIN 
                        (
                        select SUM(A.ZDHSL) RKSUMSL ,A.MATNR
                        from ZC10MMDG072 A 
                       JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "           join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS AND C.CKH NOT IN('01','02','03','04','05','06','07','08')";
            sql += "        WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            sql += @"       GROUP BY A.MATNR) I ON F.WL_CODE=I.MATNR
                        LEFT JOIN 
                        (
                        select SUM(A.ZFHSL) CKSL,A.MATNR
                        from ZC10MMDG078 A ";
            sql += "           join WZ_KCDD C ON C.KCDD_CODE = A.LGORT AND C.DWCODE = A.WERKS AND C.CKH NOT IN('01','02','03','04','05','06','07','08')";
            sql += "           JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "             WHERE A.ZSTATUS>'03' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            sql += @"         GROUP BY A.MATNR) J ON F.WL_CODE=J.MATNR

                        LEFT JOIN 
                        (
                        select SUM(A.ZFHSL) CKSUMSL ,A.MATNR
                        from ZC10MMDG078 A 
                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "           join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS AND  C.CKH NOT IN('01','02','03','04','05','06','07','08')";
            sql += "          WHERE A.ZSTATUS>'03' and substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            sql += "         GROUP BY A.MATNR) K ON F.WL_CODE=K.MATNR ";

            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " where  F.MATNR like'%" + MATNR + "%'";
            }

            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 重点物资出入库明细-分库
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MONTH"></param>
        /// <returns></returns>
        public DataTable getZDWZCRKDetail(string DKCODE, string MATNR, string MONTH)
        {
            string sql = @" select MAX(CASE WHEN  C.DW_NAME IS NULL THEN B.WEMPF ELSE  C.DW_NAME  END)  WERKS_NAME,SUM(A.ZFHSL) SL
                        from ZC10MMDG078 A
                        JOIN MSEG B ON A.MBLNR=B.MBLNR AND A.ZEILE=B.ZEILE";
            sql += "     join WZ_KCDD C ON C.KCDD_CODE = A.LGORT AND C.DWCODE = A.WERKS AND C.CKH NOT IN('01','02','03','04','05','06','07','08')";
            sql += "      LEFT JOIN WZ_DW C ON C.DW_CODE=B.WEMPF";
            sql += "     where A.MATNR='" + MATNR + "'   and  substr(A.ZCJRQ,1,6)='" + MONTH + "'  group by B.WEMPF";
            return db.GetDataTable(sql);
        }
    }
}

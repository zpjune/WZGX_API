using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class ZXKDB
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
        public DataSet GetDRKInfo(string MATNR,string info,string FacCode,int page,int limit)
        {
            
            string PartSql  = " {0} SELECT {1} FROM {2}";
            string MainSql = "（SELECT a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,SUM(a.MENGE) AS MENGE,c.ERNAM,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1,f.DW_NAME FROM ZC10MMDG072 a " +
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " WHERE a.ZSTATUS='01'";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND a.MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql += " AND e.MAKTX like'" + info + "%'";
            }
            
            string KCDDSql = "SELECT KCDD_CODE,DWCODE FROM WZ_KCDD WHERE CKH='" + FacCode + "'";
            DataTable KCDDData = db.GetDataTable(KCDDSql);
            if (KCDDData.Rows.Count > 0)
            {
                MainSql += " AND(";
                foreach (DataRow dr in KCDDData.Rows)
                {
                    MainSql += " (a.WERKS='" + dr["DWCODE"] + "'";
                    MainSql += "  AND a.LGORT='" + dr["KCDD_CODE"] + "')";
                    if (!dr.Equals(KCDDData.Rows[KCDDData.Rows.Count - 1]))
                    {
                        MainSql += " OR";
                    }
                }
                MainSql += ")";
            }
            MainSql += " GROUP BY a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,c.ERNAM,a.WERKS,c.NAME1,f.DW_NAME ORDER BY a.ZDHTZD DESC)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql+ " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }


        public DataSet GetDCKInfo(string MATNR, string info, string FacCode,int page, int limit)
        {
            string PartSql = " {0} SELECT {1} FROM {2}";
            string MainSql = "（SELECT a.ZCKTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,SUM(a.ZFHSL) AS ZFHSL,c.ERNAM,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1,f.DW_NAME FROM ZC10MMDG078 a " +
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " WHERE a.ZSTATUS='01'";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND a.MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql += " AND e.MAKTX like'" + info + "%'";
            }

            string KCDDSql = "SELECT KCDD_CODE,DWCODE FROM WZ_KCDD WHERE CKH='" + FacCode + "'";
            DataTable KCDDData = db.GetDataTable(KCDDSql);
            if (KCDDData.Rows.Count > 0)
            {
                MainSql += " AND(";
                foreach (DataRow dr in KCDDData.Rows)
                {
                    MainSql += " (a.WERKS='" + dr["DWCODE"] + "'";
                    MainSql += "  AND a.LGORT='" + dr["KCDD_CODE"] + "')";
                    if (!dr.Equals(KCDDData.Rows[KCDDData.Rows.Count - 1]))
                    {
                        MainSql += " OR";
                    }
                }
                MainSql += ")";
            }
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
        public DataTable GetFK_JYWZ(string DKCODE, string MATNR, string MATKL)
        {
            string sql = @" select sum(GESME) GESME,WERKS,WERKS_NAME,LGORT_NAME,LGORT,MAX(MATKL)MATKL,MAX(MAKTX)MAKTX,ZSTATUS,MAX(MEINS)MEINS,
                            '积压' ZT
                               ,werks,matnr,lgort 
                            from CONVERT_SWKC  ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += "where months_between(sysdate,to_date(ERDAT,'yyyy-mm-dd'))>6 AND substr(LGPLA,1,2)='"+DKCODE+"' ";
         
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
            string sql = " SELECT SUBSTR(LGPLA,3,2) AS LG,MATNR,01 AS Status FROM CONVERT_SWKC a" +
                " where 1=1" +
                " AND SUBSTR(LGPLA,0,2)='" + FacCode + "'" +
                " AND ZSTATUS='04'" +
                " AND MONTHS_BETWEEN(TO_DATE('" + DateTime.Now.ToString("yyyyMMdd") + "','yyyyMMdd'),TO_DATE(ERDAT,'yyyyMMdd'))>" + Month_between +
                " GROUP BY MATNR,SUBSTR(LGPLA,3,2)" +
                " ORDER BY SUBSTR(LGPLA,3,2)";
            return db.GetDataTable(sql);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class BBTJCXDB
    {
        DBTool db = new DBTool("");
        public DataTable GetFCLInfo(string Date)
        {
            string dateStr = string.Empty;
            string dateStr1 = string.Empty;        
            if (string.IsNullOrEmpty(Date))
            {
                dateStr = DateTime.Now.ToString("yyyyMM");
                dateStr1 = dateStr.Substring(0, 4) + "01";
            }
            else
            {
                dateStr = Convert.ToDateTime(Date).ToString("yyyyMM");
                dateStr1 = dateStr.Substring(0, 4) + "01";
            }
            string sql = "SELECT WERKS,WERKS_NAME," +
                "NVL((SELECT SUM(JE) JE FROM CONVERT_CKJE " +
                " WHERE substr( BUDAT_MKPF, 1, 6 ) ='" + dateStr + "' AND WERKS=a.WERKS GROUP BY WERKS,WERKS_NAME),0)AS DANGYUE,NVL(SUM(JE),0) AS LEIJI FROM CONVERT_CKJE a " +
                " WHERE substr( BUDAT_MKPF, 1, 6 ) >='" + dateStr1 + "' AND substr( BUDAT_MKPF, 1, 6 ) <= '" + dateStr + "'" +
                " GROUP BY WERKS,WERKS_NAME ORDER BY WERKS";
            return db.GetDataTable(sql);
        }

        public DataTable GetJYWZTJInfo(string FacCode,string MATNR)
        {
            string StrNow=DateTime.Now.ToString("yyyyMMdd");
            string sql = " SELECT SUM(a.SCJE) AS BENNIAN,a.MATNR,b.CKH," +
                //1年到3年金额子查询开始
                "( SELECT  SUM(c.SCJE) AS BENNIAN FROM CONVERT_SWKCDETAIL c " +
                " JOIN WZ_KCDD d ON c.WERKS = d.DWCODE AND c.LGORT = d.KCDD_CODE" +
                " WHERE 1=1" +
                " AND MONTHS_BETWEEN(TO_DATE('" + StrNow + "','yyyyMMdd'),TO_DATE(c.ERDATE,'yyyyMMdd'))>12" +
                " AND MONTHS_BETWEEN(TO_DATE('" + StrNow + "','yyyyMMdd'),TO_DATE(c.ERDATE,'yyyyMMdd'))<=36" +
                " AND c.MATNR = a.MATNR " +
                " AND d.CKH = b.CKH " +
                "{0}" +
                " GROUP BY c.MATNR,d.CKH)" +
                //子查询结束
                " AS SANNIAN," +
                //三年以上金额子查询开始
                " (SELECT SUM( e.SCJE ) AS BENNIAN FROM CONVERT_SWKCDETAIL e" +
                " JOIN WZ_KCDD f ON e.WERKS = f.DWCODE AND e.LGORT = f.KCDD_CODE" +
                " WHERE 1=1" +
                " AND MONTHS_BETWEEN(TO_DATE('" + StrNow + "','yyyyMMdd'),TO_DATE(e.ERDATE,'yyyyMMdd'))>36" +
                " AND e.MATNR = a.MATNR" +
                " AND f.CKH = b.CKH " +
                "{1}" +
                " GROUP BY e.MATNR,f.CKH )" +
                //子查询结束
                " AS SANNIANYISHANG " +
                " FROM CONVERT_SWKCDETAIL a" +
                " JOIN WZ_KCDD b ON a.WERKS = b.DWCODE AND a.LGORT = b.KCDD_CODE " +
                " WHERE 1=1 " +
                " AND MONTHS_BETWEEN(TO_DATE('" + StrNow + "','yyyyMMdd'),TO_DATE(a.ERDATE,'yyyyMMdd'))<=12" +
                " {2}" +
                " GROUP BY a.MATNR,b.CKH ORDER BY b.CKH";
            if (!string.IsNullOrEmpty(FacCode))
            {
                sql = string.Format(sql, " AND d.CKH='" + FacCode + "'{0}", " AND f.CKH='" + FacCode + "'{1}", " AND b.CKH='" + FacCode + "'{2}");
            }
            else
            {
                sql = string.Format(sql, "{0}", "{1}", "{2}");
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql = string.Format(sql, " AND c.MATNR='" + MATNR + "'", " AND e.MATNR='" + MATNR + "'", " AND a.MATNR='" + MATNR + "'");
            }
            else
            {
                sql = string.Format(sql, "", "", "");
            }
            return db.GetDataTable(sql);
        }
    }
}

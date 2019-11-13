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
    }
}

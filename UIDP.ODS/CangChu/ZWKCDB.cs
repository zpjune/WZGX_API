using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class ZWKCDB
    {
        DBTool db = new DBTool("");

        public DataTable GetTotalInfo(string MATKL, string CODE,int level = 0)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql = "select MATKL,SUM(SALK3)AS SALK3,PMNAME FROM CONVERT_ZWKC where MATKL='" + MATKL + "'GROUP BY MATKL,PMNAME";
            }  
            else
            {
                switch (level)
                {
                    case 0:
                        sql = "select DLCODE,DLNAME,SUM(SALK3)AS SALK3 FROM CONVERT_ZWKC GROUP BY DLCODE,DLNAME ORDER BY DLCODE";
                        break;
                    case 1:
                        sql = "select DLCODE,ZLCODE,ZLNAME,SUM(SALK3)AS SALK3 FROM CONVERT_ZWKC WHERE DLCODE='"+CODE+"' GROUP BY ZLCODE,ZLNAME,DLCODE ORDER BY ZLCODE";
                        break;
                    case 2:
                        sql = "select DLCODE,ZLCODE,XLCODE,XLNAME,SUM(SALK3)AS SALK3 FROM CONVERT_ZWKC WHERE ZLCODE='"+CODE+"' GROUP BY XLCODE,ZLCODE,DLCODE,XLNAME ORDER BY XLCODE";
                        break;
                    case 3:
                        sql = "  select MATKL,PMNAME,SUM(SALK3)AS SALK3 FROM CONVERT_ZWKC WHERE XLCODE='"+CODE+"' GROUP BY MATKL,PMNAME ORDER BY MATKL";
                        break;
                }
            }
            return db.GetDataTable(sql);
        }
    }
}

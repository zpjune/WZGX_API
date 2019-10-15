using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class SWKCDB
    {
        DBTool db = new DBTool("");
        public DataTable GetFacInfo(string WERKS,string LGORT,string LGORT_NAME)
        {
            string sql = " SELECT WERKS,MEINS,SUM(GESME)AS GESME,ZSTATUS,LGORT_NAME FROM CONVERT_SWKC where 1=1";
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " AND WERKS='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(LGORT))
            {
                sql += " AND LGORT='" + LGORT + "'";
            }
            if (!string.IsNullOrEmpty(LGORT_NAME))
            {
                sql += " AND LGORT_NAME='" + LGORT_NAME + "'";
            }
            sql += " group BY WERKS,MEINS,ZSTATUS,LGORT_NAME ORDER BY WERKS";
            return db.GetDataTable(sql);
        }

        public DataTable GetCompositeInfo(string WERKS,string LGORT, string LGORT_NAME,string MATNR,string MAKTX)
        {
            string sql = " select * from CONVERT_SWKC where 1=1";
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " AND WERKS='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(LGORT))
            {
                sql += " AND LGORT='" + LGORT + "'";
            }
            if (!string.IsNullOrEmpty(LGORT_NAME))
            {
                sql += " AND LGORT_NAME='" + LGORT_NAME + "'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " AND LGORT_NAME='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(MAKTX))
            {
                sql += " AND LGORT_NAME like'" + MAKTX + "%'";
            }
            return db.GetDataTable(sql);
        }

        public DataTable GetAllInfo(string MATKL,int level=0)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += "select * from CONVERT_SWKC where MATKL='" + MATKL + "'";
            }
            else
            {
                sql = "SELECT {0} FROM CONVERT_SWKC a LEFT JOIN WZ_WLZ b ON a.MATKL=b.PMCODE GROUP BY {1} ORDER BY {2}";
                switch (level)
                {
                    case 0:
                        sql = string.Format(sql, "b.DLCODE,b,DLNAME", "b.DLCODE,b,DLNAME", "b.DLCODE");
                        break;
                    case 1:
                        sql= string.Format(sql, "b.ZLCODE,b,ZLNAME", "b.ZLCODE,b,ZLNAME", "b.ZLCODE");
                        sql += " AND b.DLCODE='" + MATKL + "'";
                        break;
                    case 2:
                        sql = string.Format(sql, "b.XLCODE,b,XLNAME", "b.XLCODE,b,XLNAME", "b.XLCODE");
                        sql += " AND b.ZLCODE='" + MATKL + "'";
                        break;
                    case 3:
                        sql = string.Format(sql, "b.PMCODE,b,PMNAME", "b.PMCODE,b,PMNAME", "b.PMCODE");
                        sql += " AND b.XLCODE='" + MATKL + "'";
                        break;
                }
            }
            return db.GetDataTable(sql);
        }
    }
}

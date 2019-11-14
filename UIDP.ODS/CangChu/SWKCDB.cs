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
            string sql = " SELECT WERKS,MEINS,SUM(GESME)AS GESME,ZSTATUS,LGORT_NAME FROM CONVERT_SWKC where (KCTYPE IS NULL OR KCTYPE<>3)";
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

        public DataSet GetCompositeInfo(string WERKS,string LGORT, string LGORT_NAME,string MATNR,string MAKTX,int page,int limit)
        {
            string Mainsql = " select * from CONVERT_SWKC where (KCTYPE IS NULL OR KCTYPE<>3)";
            if (!string.IsNullOrEmpty(WERKS))
            {
                Mainsql += " AND WERKS='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(LGORT))
            {
                Mainsql += " AND LGORT='" + LGORT + "'";
            }
            if (!string.IsNullOrEmpty(LGORT_NAME))
            {
                Mainsql += " AND LGORT_NAME='" + LGORT_NAME + "'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                Mainsql += " AND MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(MAKTX))
            {
                Mainsql += " AND MAKTX like'" + MAKTX + "%'";
            }
            string PartSql = "SELECT {0} FROM (SELECT t.*,ROWNUM AS rn FROM ({1})t {2})tt {3}";
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("DataSql", string.Format(PartSql, "*", Mainsql, " WHERE ROWNUM<" + ((page * limit) + 1), " WHERE tt.rn>" + ((page - 1) * limit)));
            d.Add("TotalSql", string.Format(PartSql, "COUNT(*) AS TOTAL", Mainsql,"",""));
            
            return db.GetDataSet(d);
        }

        public DataTable GetAllInfo(string MATKL,string code,int level=0)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += "select * from CONVERT_SWKC where MATKL='" + MATKL + "' AND KCTYPE<>3";
            }
            else
            {
                sql = "SELECT {0} FROM CONVERT_SWKC a LEFT JOIN WZ_WLZ b ON a.MATKL=b.PMCODE where {1}  GROUP BY {2} ORDER BY {3}";
                switch (level)
                {
                    case 0:
                        sql = string.Format(sql, "b.DLCODE,b.DLNAME", "(KCTYPE IS NULL OR KCTYPE<>3)", "b.DLCODE,b. DLNAME", "b.DLCODE");
                        break;
                    case 1:
                        sql= string.Format(sql, "b.ZLCODE,b.ZLNAME", "b.DLCODE='" + code + "'","b.ZLCODE,b.ZLNAME", "b.ZLCODE");
                        break;
                    case 2:
                        sql = string.Format(sql, "b.XLCODE,b.XLNAME", "b.ZLCODE='" + code + "'", "b.XLCODE,b.XLNAME", "b.XLCODE");
                        break;
                    case 3:
                        sql = string.Format(sql, "b.PMCODE,b.PMNAME,a.MEINS,a.GESME,a.LGORT_NAME", "b.XLCODE='" + code + "'", "b.PMCODE,b.PMNAME,a.MEINS,a.GESME,a.LGORT_NAME", "b.PMCODE");
                        break;
                }
            }
            return db.GetDataTable(sql);
        }

        public DataSet GetWLTotalInfo(string MATNR,string MAKTX,int page,int limit)
        {
            string Mainsql = " SELECT MATNR,MAKTX,SUM(GESME) AS GESME,MEINS,ZSTATUS FROM CONVERT_SWKC WHERE (KCTYPE IS NULL OR KCTYPE<>3)";
            if (!string.IsNullOrEmpty(MATNR))
            {
                Mainsql += " AND MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(MAKTX))
            {
                Mainsql += " AND MAKTX like'%" + MAKTX + "%'";
            }
            Mainsql += " GROUP BY MATNR,MAKTX,MEINS,ZSTATUS ORDER BY MATNR";
            string TotalSql = "SELECT {0} FROM (SELECT ROWNUM AS RN,t.* FROM({1})t {2} )tt {3}";
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DataSql", string.Format(TotalSql, "*", Mainsql, " WHERE ROWNUM<" + ((page * limit)+1), " WHERE tt.RN>" + ((page - 1) * limit)));
            list.Add("CountSql", string.Format(TotalSql, "COUNT(*) AS TOTOAL", Mainsql, "", ""));
            return db.GetDataSet(list);
        }

        public DataTable GetWLDetail(string MATNR)
        {
            string sql = " SELECT WERKS_NAME,LGORT_NAME,SUM(GESME) AS GESME,MEINS,ZSTATUS FROM CONVERT_SWKC WHERE (KCTYPE IS NULL OR KCTYPE<>3) " +
                "AND MATNR='" + MATNR + "' GROUP BY WERKS_NAME,LGORT_NAME,MEINS,ZSTATUS ORDER BY WERKS_NAME";
            return db.GetDataTable(sql);
        }
    }
}

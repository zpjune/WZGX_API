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
                        //sql = " SELECT ROWNUM,t.* FROM(select DLCODE,DLNAME,SUM(SALK3)AS SALK3 FROM CONVERT_ZWKC  GROUP BY DLCODE,DLNAME ORDER BY DLCODE )t WHERE ROWNUM<page*limit " +
                        //    "minus " +
                        //    "SELECT ROWNUM,t.* FROM(select DLCODE,DLNAME,SUM(SALK3)AS SALK3 FROM CONVERT_ZWKC  GROUP BY DLCODE,DLNAME ORDER BY DLCODE )t WHERE ROWNUM<(page-1)*limit+1";
                        // 上面注释为数据库分页方法，使用数据库分页需要返回dataset，还需要使用selec coun(*)查询总条数返回
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

        public DataTable GetFacMoney(string BWKEY,string BWKEY_NAME)
        {
            string sql = "select BWKEY,BWKEY_NAME,SUM(SALK3) as SALK3 from CONVERT_ZWKC  WHERE 1=1";
            if (!string.IsNullOrEmpty(BWKEY))
            {
                sql += " AND BWKEY LIKE'" + BWKEY + "%'";
            }
            if (!string.IsNullOrEmpty(BWKEY_NAME))
            {
                sql += " AND BWKEY_NAME='" + BWKEY_NAME + "'";
            }
            sql += " GROUP BY BWKEY,BWKEY_NAME ORDER BY BWKEY";
            return db.GetDataTable(sql);
        }

        public DataTable GetExportCompositeInfo()
        {
            string sql = "select BWKEY,BWKEY_NAME,DLNAME,ZLNAME,XLNAME,PMNAME,SALK3 from CONVERT_ZWKC ";
            return db.GetDataTable(sql);
        }
        public DataSet GetCompositeInfo(string BWKEY,int type,string CODE,int page,int limit)
        {
            string PartSql = " select * from CONVERT_ZWKC where 1=1";
            string totalsql = "select count(*) AS TOTAL from CONVERT_ZWKC WHERE 1=1";
            if (!string.IsNullOrEmpty(BWKEY))
            {
                PartSql += " AND BWKEY LIKE'" + BWKEY + "%'";
                totalsql += " AND BWKEY LIKE'" + BWKEY + "%'";
            }
            if (!string.IsNullOrEmpty(CODE))
            {
                switch (type)
                {
                    case 0:
                        PartSql += " AND DLCODE='" + CODE + "'";
                        totalsql += " AND DLCODE='" + CODE + "'";
                        break;
                    case 1:
                        PartSql += " AND ZLCODE='" + CODE + "'";
                        totalsql += " AND ZLCODE='" + CODE + "'";
                        break;
                    case 2:
                        PartSql += " AND XLCODE='" + CODE + "'";
                        totalsql += " AND XLCODE='" + CODE + "'";
                        break;
                    case 3:
                        PartSql += " AND MATKL='" + CODE + "'";
                        totalsql += " AND MATKL='" + CODE + "'";
                        break;
                }
            }
            string CompleteSql = " SELECT ROWNUM,t.* FROM(" + PartSql + ")t where ROWNUM<" + page * limit
                + "minus" + " SELECT ROWNUM,t.* FROM(" + PartSql + ")t where ROWNUM<" + ((page - 1) * limit+1);
            
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("CompleteSql", CompleteSql);
            d.Add("totalsql", totalsql);
            return db.GetDataSet(d);  
        }
    }
}

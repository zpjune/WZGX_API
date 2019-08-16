using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class KCDDWHDB
    {
        DBTool db = new DBTool("");

        public DataTable GetKCDDInfo(string KCDD_CODE, string KCDD_NAME)
        {
            string sql = "select * from WZ_KCDD where 1=1";
            if (!String.IsNullOrEmpty(KCDD_CODE))
            {
                sql += " AND KCDD_CODE='" + KCDD_CODE + "'";
            }
            if (!String.IsNullOrEmpty(KCDD_NAME))
            {
                sql += " AND KCDD_NAME='" + KCDD_NAME + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateKCDDInfo(Dictionary<string, string> d)
        {
            string sql = "insert into WZ_KCDD(KCDD_CODE,KCDD_NAME)VALUES('" + d["KCDD_CODE"] + "','" + d["KCDD_NAME"] +"')";
            return db.ExecutByStringResult(sql);
        }

        public string EditKCDDInfo(Dictionary<string, string> d)
        {
            string sql = " UPDATE WZ_KCDD SET KCDD_NAME='" + d["KCDD_NAME"] + "' WHERE KCDD_CODE='" + d["KCDD_CODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public string DelKCDDInfo(Dictionary<string, object> d)
        {
            string sql = "DELETE WZ_KCDD WHERE KCDD_NAME='" + d["KCDD_NAME"] + "'";
            return db.ExecutByStringResult(sql);
        }
    }
}

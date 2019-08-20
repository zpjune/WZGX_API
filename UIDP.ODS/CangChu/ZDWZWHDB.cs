using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class ZDWZWHDB
    {
        DBTool db = new DBTool("");
        public DataTable GetZDWZWHInfo(string WL_LOCATIONCODE,string WLZ_CODE,string WL_CODE)
        {
            string sql = " SELECT a.*,b.KCDD_NAME,c.WL_NAME FROM WZ_ZDWZWH a " +
                "LEFT JOIN WZ_KCDD b ON a.WL_LOCATIONCODE = b.KCDD_CODE " +
                "LEFT JOIN WZ_ZDWZPZ c ON a.WL_CODE = c.WL_CODE" +
                " WHERE 1=1";
            if (!String.IsNullOrEmpty(WL_LOCATIONCODE))
            {
                sql += " AND a.WL_LOCATIONCODE='" + WL_LOCATIONCODE + "'";
            }
            if (!String.IsNullOrEmpty(WL_CODE))
            {
                sql += " AND a.WL_CODE='" + WL_CODE + "'";
            }
            if (!String.IsNullOrEmpty(WLZ_CODE))
            {
                sql += " AND a.WLZ_CODE='" + WLZ_CODE + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateZDWZWHInfo(Dictionary<string,string> d)
        {
            string sql = "INSERT INTO WZ_ZDWZWH (ID,WLZ_CODE,WL_CODE,WL_LOCATIONCODE,MAXHAVING,MINHAVING)VALUES('" +
                d["ID"] + "','" + d["WLZ_CODE"] + "','"+d["WL_CODE"]+"','" + d["WL_LOCATIONCODE"] + "','" + d["MAXHAVING"] + "','" + d["MINHAVING"] + "')";
            return db.ExecutByStringResult(sql);
        }

        public string EditZDWZWHInfo(Dictionary<string,string> d)
        {
            string sql = "UPDATE WZ_ZDWZWH SET WLZ_CODE='" + d["WLZ_CODE"] + "',WL_LOCATIONCODE='" + d["WL_LOCATIONCODE"] + "',MAXHAVING='" + d["MAXHAVING"] + "',MINHAVING='" + d["MINHAVING"] + "',WL_CODE='"+d["WL_CODE"]+"' ";
            sql += " WHERE ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string DelZDWZWHInfo(Dictionary<string,string> d)
        {
            string sql = "DELETE WZ_ZDWZWH WHERE ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataTable GetKCDDInfo()
        {
            string sql = "SELECT * FROM WZ_KCDD";
            return db.GetDataTable(sql);
        }

        public DataTable GetWLZCODE()
        {
            string sql = "SELECT WLZ_CODE FROM WZ_ZDWZPZ";
            return db.GetDataTable(sql);
        }
        public DataTable GetWZCODE(string WLZ_CODE)
        {
            string sql = "SELECT WL_CODE,WL_NAME FROM WZ_ZDWZPZ WHERE WLZ_CODE='" + WLZ_CODE + "'";
            return db.GetDataTable(sql);
        }
    }
}

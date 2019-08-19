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
        public DataTable GetZDWZWHInfo(string WL_LOCATIONCODE, string WL_CODE)
        {
            string sql = " SELECT * FROM WZ_ZDWZWH WHERE 1=1";
            if (!String.IsNullOrEmpty(WL_LOCATIONCODE))
            {
                sql += " AND WL_LOCATIONCODE='" + WL_LOCATIONCODE + "'";
            }
            if (!String.IsNullOrEmpty(WL_CODE))
            {
                sql += " AND WL_CODE='" + WL_CODE + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateZDWZWHInfo(Dictionary<string,string> d)
        {
            string sql = "INSERT INTO WZ_ZDWZWH (ID,WLZ_CODE,WL_CODE,WL_LOCATIONCODE,MAXHAVING,MINHAVING)VALUES('" +
                d["ID"] + "','" + d["WLZ_CODE"] + "','" + d["WL_LOCATIONCODE"] + "','" + d["MAXHAVING"] + "','" + d["MINHAVING"] + "')";
            return db.ExecutByStringResult(sql);
        }

        public string EditZDWZWHInfo(Dictionary<string,string> d)
        {
            string sql = "UPDATE WZ_ZDWZWH SET WLZ_CODE='" + d["WLZ_CODE"] + "' WL_LOCATIONCODE='" + d["WL_LOCATIONCODE"] + "' MAXHAVING='" + d["MAXHAVING"] + "' MINHAVING='" + d["MINHAVING"] + "'";
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
    }
}

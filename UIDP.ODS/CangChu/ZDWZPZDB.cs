using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class ZDWZPZDB
    {
        DBTool db = new DBTool("");

        public DataTable GetZDWZPZInfo(string WLZ_CODE, string WL_CODE,string WL_NAME)
        {
            string sql = "select * from WZ_ZDWZPZ where 1=1";
            if (!String.IsNullOrEmpty(WLZ_CODE))
            {
                sql += " AND WLZ_CODE='" + WLZ_CODE + "'";
            }
            if (!String.IsNullOrEmpty(WL_CODE))
            {
                sql += " AND WL_CODE='" + WL_CODE + "'";
            }
            if (!String.IsNullOrEmpty(WL_NAME))
            {
                sql += " AND WL_NAME='" + WL_NAME + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateZDWZPZInfo(Dictionary<string, string> d)
        {
            string sql = "insert into WZ_ZDWZPZ(WLZ_CODE,WL_CODE,WL_NAME,ID)VALUES('" + d["WLZ_CODE"] + "','" + d["WL_CODE"] + "','"+d["WL_NAME"]+"','"+d["ID"]+"')";
            return db.ExecutByStringResult(sql);
        }

        public string EditZDWZPZInfo(Dictionary<string, string> d)
        {
            string sql = " UPDATE WZ_ZDWZPZ SET WL_CODE='" + d["WL_CODE"] + "', WL_NAME='"+d["WL_NAME"]+"'" +"WHERE WLZ_CODE='" + d["WLZ_CODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public string DelZDWZPZInfo(Dictionary<string, object> d)
        {
            string sql = "DELETE WZ_ZDWZPZ WHERE ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataTable GetPMCODE()
        {
            string sql = "SELECT PMCODE,PMNAME from WZ_WLZ";
            return db.GetDataTable(sql);
        }

        public DataTable GetRepeat(string WLZ_CODE,string WL_CODE)
        {
            string sql = "SELECT 1 FROM WZ_ZDWZPZ WHERE WLZ_CODE='" + WLZ_CODE + "' AND WL_CODE='"+ WL_CODE+"'";
            return db.GetDataTable(sql);
        }
    }
}

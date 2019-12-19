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
            string sql = "select a.* from WZ_ZDWZPZ a " +
                //"left join WZ_WLZ b on a.WLZ_CODE=b.PMCODE " +
                "where 1=1";
            if (!String.IsNullOrEmpty(WLZ_CODE))
            {
                sql += " AND a.WLZ_CODE='" + WLZ_CODE + "'";
            }
            if (!String.IsNullOrEmpty(WL_CODE))
            {
                sql += " AND a.WL_CODE='" + WL_CODE + "'";
            }
            if (!String.IsNullOrEmpty(WL_NAME))
            {
                sql += " AND a.WL_NAME='" + WL_NAME + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateZDWZPZInfo(Dictionary<string, string> d)
        {
            string sql = "insert into WZ_ZDWZPZ(WL_CODE,WL_NAME,ID,WL_SORT)VALUES('" + d["WL_CODE"] + "','"+d["WL_NAME"]+"','"+d["ID"]+"','"+d["WL_SORT"]+"')";
            return db.ExecutByStringResult(sql);
        }

        public string EditZDWZPZInfo(Dictionary<string, string> d)
        {
            string sql = " UPDATE WZ_ZDWZPZ SET WL_CODE='" + d["WL_CODE"] + "', WL_NAME='"+d["WL_NAME"]+ "',WL_SORT='"+d["WL_SORT"]+"'" + " WHERE ID='"+d["ID"]+"'";
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

        public DataTable GetRepeat(string WL_CODE,string YEAR)
        {
            string sql = "SELECT 1 FROM WZ_ZDWZPZ WHERE WL_CODE='"+ WL_CODE+"'AND YEAR='"+YEAR+"'";
            return db.GetDataTable(sql);
        }

        public DataTable GetParentNode()
        {
            string sql = "SELECT DISTINCT DLCODE,DLNAME from WZ_WLZ ORDER BY DLCODE";
            return db.GetDataTable(sql);
        }
        public DataTable GetChildrenNode(string flagID, string DLCODE, string ZLCODE, string XLCODE)
        {
            string sql = string.Empty;
            switch (flagID)
            {
                case "Parent":
                    sql = "SELECT DISTINCT ZLCODE,ZLNAME from  WZ_WLZ WHERE DLCODE='" + DLCODE + "'ORDER BY ZLCODE";
                    break;
                case "ZLNode":
                    sql = "SELECT DISTINCT XLCODE,XLNAME FROM WZ_WLZ WHERE DLCODE='" + DLCODE + "'AND ZLCODE='" + ZLCODE + "' ORDER BY XLCODE";
                    break;
                case "XLNode":
                    sql = " SELECT DISTINCT PMCODE,PMNAME FROM WZ_WLZ WHERE DLCODE='" + DLCODE + "'AND ZLCODE='" + ZLCODE + "'AND XLCODE='" + XLCODE + "' ORDER BY PMCODE";
                    break;
            }
            return db.GetDataTable(sql);
        }

        public DataSet GetNode(string PMCODE)
        {
            string DLCODE = PMCODE.Substring(0, 2);
            string ZLCODE = PMCODE.Substring(0, 4);
            string XLCODE = PMCODE.Substring(0, 6);
            Dictionary<string, string> list = new Dictionary<string, string>();
            string DLsql = "SELECT DISTINCT DLCODE,DLNAME from WZ_WLZ ORDER BY DLCODE";
            string ZLsql= "SELECT DISTINCT ZLCODE,ZLNAME from  WZ_WLZ WHERE DLCODE='" + DLCODE + "'ORDER BY ZLCODE";
            string XLsql= "SELECT DISTINCT XLCODE,XLNAME FROM WZ_WLZ WHERE DLCODE='" + DLCODE + "'AND ZLCODE='" + ZLCODE + "' ORDER BY XLCODE";
            string PMsql= " SELECT DISTINCT PMCODE,PMNAME FROM WZ_WLZ WHERE DLCODE='" + DLCODE + "'AND ZLCODE='" + ZLCODE + "'AND XLCODE='" + XLCODE + "' ORDER BY PMCODE";
            list.Add("DLsql", DLsql);
            list.Add("ZLsql", ZLsql);
            list.Add("XLsql", XLsql);
            list.Add("PMsql", PMsql);
            return db.GetDataSet(list);
        }

    }
}

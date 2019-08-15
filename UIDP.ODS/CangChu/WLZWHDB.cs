using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class WLZWHDB
    {
        DBTool db = new DBTool("");
        public DataTable GetParentWLZList(string WLZCODE,string WLZNAME)
        {
            string sql = string.Empty;
            
            if(String.IsNullOrEmpty(WLZNAME)&& String.IsNullOrEmpty(WLZCODE))
            {
                sql+= "select DISTINCT DLCODE,DLNAME from WZ_WLZ where 1=1 ORDER BY DLCODE";
            }
            else
            {
                sql += "select DISTINCT DLCODE,DLNAME from WZ_WLZ where 1=1";
                if (!String.IsNullOrEmpty(WLZCODE))
                {

                    sql += " AND PMCODE LIKE '%" + WLZCODE + "'";
                }
                if (!String.IsNullOrEmpty(WLZNAME))
                {
                    sql += " AND PMNAME LIKE '%" + WLZNAME + "'";
                }
            }
            return db.GetDataTable(sql);
        }

        public DataTable GetChildrenWLZList(string DLCODE, string ZLCODE, string XLCODE, int level)
        {
            string sql = string.Empty;
            switch (level)
            {
                case 0://查中类
                    sql += " select DISTINCT DLCODE,DLNAME,ZLCODE,ZLNAME from WZ_WLZ where 1=1 ";
                    sql += " AND DLCODE='" + DLCODE + "'";
                    sql += " ORDER BY ZLCODE";
                    //sql += " AND DLCODE='" + DLCODE + "'";
                    //sql += " AND ZLCODE IS NOT NULL AND (XLCODE IS NULL OR XLCODE<>'')";
                    break;
                case 1:
                    sql += " select DISTINCT DLCODE,DLNAME,ZLCODE,ZLNAME,XLCODE,XLNAME from WZ_WLZ where 1=1 ";
                    sql += " AND DLCODE='" + DLCODE + "'";
                    sql += " AND ZLCODE='" + ZLCODE + "'";
                    sql += " ORDER BY XLCODE";
                    //sql += " AND DLCODE='" + DLCODE + "' AND ZLCODE='" + ZLCODE + "'";//查小类
                    //sql += " AND XLCODE IS NOT NULL";
                    //sql += " AND (PMCODE IS NULL OR PMCODE='')";
                    break;
                case 2:
                    sql += " select * from WZ_WLZ where 1=1";
                    sql += " AND DLCODE='" + DLCODE + "'";
                    sql += " AND ZLCODE='" + ZLCODE + "'";
                    sql += " AND XLCODE='" + XLCODE + "'";
                    sql += " ORDER BY XLCODE";
                    //sql += " AND DLCODE='" + DLCODE + "' AND ZLCODE='" + ZLCODE + "' AND XLCODE='" + XLCODE + "'";
                    //sql += " AND LENGTH(PMCODE)=LENGTH('" + DLCODE + ZLCODE + XLCODE + "')";
                    //sql += " AND (PMCODE IS NOT NULL OR PMCODE<>'')";
                    break;
                //case 3:
                //    sql += " AND PMCODE LIKE'" + DLCODE + ZLCODE + XLCODE + "%'";
                //    sql += " AND LENGTH(PMCODE)>LENGTH('" + DLCODE + ZLCODE + XLCODE + "')";
                //    break;
                default:
                    throw new Exception("传入参数异常！");
            }
            return db.GetDataTable(sql);
        }


        public string editNode(Dictionary<string,object> d)
        {
            string sql = "UPDATE WZ_WLZ SET DLCODE='" + d["DLCODE"] + "',";
            sql += " DLNAME='" + d["DLNAME"] + "',";
            sql += " ZLCODE='" + d["ZLCODE"] + "',";
            sql += " ZLNAME='" + d["ZLNAME"] + "',";
            sql += " XLCODE='" + d["XLCODE"] + "',";
            sql += " XLNAME='" + d["XLNAME"] + "',";
            sql += " PMCODE='" + d["PMCODE"] + "',";
            sql += " PMNAME='" + d["PMNAME"] + "',";
            sql += " XHGGGF='" + d["XHGGGF"] + "',";
            sql += " JBJLDW='" + d["JBJLDW"] + "'";
            sql += " WHERE ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataSet getOptions()
        {
            string sql = "SELECT DISTINCT DLNAME,DLCODE FROM WZ_WLZ";
            string sql1 = " SELECT DISTINCT ZLNAME,ZLCODE FROM WZ_WLZ";
            string sql2 = " SELECT DISTINCT XLNAME,XLCODE FROM WZ_WLZ";
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("DL", sql);
            d.Add("ZL", sql1);
            d.Add("XL", sql2);
            return db.GetDataSet(d);
        }

        public string delNode(string id)
        {
            string sql = "DELETE FROM WZ_WLZ WHERE ID='" + id + "'";
            return db.ExecutByStringResult(sql);
        }

        public string createNode(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO WZ_WLZ(ID,DLCODE,DLNAME,ZLCODE,ZLNAME,XLCODE,XLNAME,PMCODE,PMNAME,XHGGGF,JBJLDW)VALUES(";
            sql += SQLStr(d["ID"]);
            sql += SQLStr(d["DLCODE"]);
            sql += SQLStr(d["DLNAME"]);
            sql += SQLStr(d["ZLCODE"]);
            sql += SQLStr(d["ZLNAME"]);
            sql += SQLStr(d["XLCODE"]);
            sql += SQLStr(d["XLNAME"]);
            sql += SQLStr(d["PMCODE"]);
            sql += SQLStr(d["PMNAME"]);
            sql += SQLStr(d["XHGGGF"]);
            sql += SQLStr(d["JBJLDW"]);
            sql = sql.TrimEnd(',');
            sql += ")";
            return db.ExecutByStringResult(sql);
        }
        public string SQLStr(object str)
        {
            if (str.ToString() != "")
            {
                return "'" + str + "',";
            }
            else
            {
                return "'null',";
            }
        }
    }
}

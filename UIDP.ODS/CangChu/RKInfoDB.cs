using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class RKInfoDB
    {
        DBTool db = new DBTool("");
        public DataTable GetRKInfo(string RKTime, string LocationNumber)
        {
            string sql = "SELECT * FROM WZ_Warehousing WHERE 1=1";
            if (RKTime != null)
            {
                sql += " AND DATEDIFF(mm,RK_Time,'" + RKTime + "')=0";
            }
            if (LocationNumber != null)
            {
                sql += " AND RK_LocationNumber='" + LocationNumber + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateRKInfo(Dictionary<string,object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO WZ_Warehousing(RK_ID, RK_ClassCode, RK_Code, RK_Describe, RK_Measurement, RK_Quantity, RK_Time, RK_Company, RK_Custodian, RK_Location, RK_LocationNumber, RK_Remark)VALUES(");
            sb.Append(strSQL(d["RK_ID"]));
            sb.Append(strSQL(d["RK_CLASSCODE"]));
            sb.Append(strSQL(d["RK_CODE"]));
            sb.Append(strSQL(d["RK_DESCRIBE"]));
            sb.Append(strSQL(d["RK_MEASUREMENT"]));
            sb.Append(d["RK_QUANTITY"] +",");
            sb.Append("TO_DATE('"+d["RK_TIME"]+ "','yyyy-mm-dd hh24:mi:ss'),");
            sb.Append(strSQL(d["RK_COMPANY"]));
            sb.Append(strSQL(d["RK_CUSTODIAN"]));
            sb.Append(strSQL(d["RK_LOCATION"]));
            sb.Append(strSQL(d["RK_LOCATIONNUMBER"]));
            sb.Append(strSQL(d["RK_REMARK"]));
            string sql = sb.ToString();
            sql = sql.TrimEnd(',');
            sql += ")";
            return db.ExecutByStringResult(sql);
        }
        public string DeleteRKInfo(Dictionary<string,object> d)
        {
            string sql = "DELETE WZ_Warehousing WHERE RK_ID='" + d["RK_ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string UpdateRKInfo(Dictionary<string, object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE WZ_Warehousing SET ");
            sb.Append("RK_ClassCode=" + strSQL(d["RK_CLASSCODE"]));
            sb.Append("RK_Code=" + strSQL(d["RK_CODE"]));
            sb.Append("RK_Describe=" + strSQL(d["RK_DESCRIBE"]));
            sb.Append("RK_Measurement=" + strSQL(d["RK_MEASUREMENT"]));
            sb.Append("RK_Quantity="+d["RK_QUANTITY"] +",");
            sb.Append("RK_TIME=TO_DATE('" + d["RK_TIME"] + "','yyyy-mm-dd hh24:mi:ss'),");
            sb.Append("RK_COMPANY=" + strSQL(d["RK_COMPANY"]));
            sb.Append("RK_CUSTODIAN=" + strSQL(d["RK_CUSTODIAN"]));
            sb.Append("RK_LOCATION=" + strSQL(d["RK_LOCATION"]));
            sb.Append("RK_LOCATIONNUMBER=" + strSQL(d["RK_LOCATIONNUMBER"]));
            sb.Append("RK_REMARK=" + strSQL(d["RK_REMARK"]));
            string sql = sb.ToString();
            sql = sql.TrimEnd(',');
            sql += " WHERE RK_ID='" + d["RK_ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string strSQL(object t)
        {
            string str = string.Empty;
            if (t != ""&&t!=null)
            {
                str = "'" + t.ToString() + "',";
                return str;
            }
            else
            {
                return "null,";
            }
        }
    }
}

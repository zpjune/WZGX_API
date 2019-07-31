using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class CKInfoDB
    {
        DBTool db = new DBTool("");
        public DataTable GetCKInfo(string CKTime, string LocationNumber)
        {
            string sql = "SELECT * FROM WZ_OutOfStock WHERE 1=1";
            if (CKTime != null)
            {
                sql += " AND DATEDIFF(mm,CK_Time,'" + CKTime + "')=0";
            }
            if (LocationNumber != null)
            {
                sql += " AND CK_LocationNumber='" + LocationNumber + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateCKInfo(Dictionary<string, object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO WZ_OutOfStock(CK_ID, CK_ClassCode, CK_Code, CK_Describe, CK_Measurement,CK_Time, CK_Company, CK_Custodian, CK_Location, CK_LocationNumber, CK_Remark)VALUES(");
            sb.Append(strSQL(d["CK_ID"]));
            sb.Append(strSQL(d["CK_CLASSCODE"]));
            sb.Append(strSQL(d["CK_CODE"]));
            sb.Append(strSQL(d["CK_DESCRIBE"]));
            sb.Append(strSQL(d["CK_MEASUREMENT"]));
            //sb.Append(d["CK_Quantity"] + ",");
            sb.Append("TO_DATE('"+d["CK_TIME"]+ "','yyyy-mm-dd hh24:mi:ss'),");
            sb.Append(strSQL(d["CK_COMPANY"]));
            sb.Append(strSQL(d["CK_CUSTODIAN"]));
            sb.Append(strSQL(d["CK_LOCATION"]));
            sb.Append(strSQL(d["CK_LOCATIONNUMBER"]));
            sb.Append(strSQL(d["CK_REMARK"]));
            string sql = sb.ToString();
            sql = sql.TrimEnd(',');
            sql += ")";
            return db.ExecutByStringResult(sql);
        }
        public string DeleteCKInfo(Dictionary<string, object> d)
        {
            string sql = "DELETE WZ_OutOfStock WHERE CK_ID='" + d["CK_ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string UpdateCKInfo(Dictionary<string, object> d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE WZ_OutOfStock SET ");
            sb.Append("CK_CLASSCODE=" + strSQL(d["CK_CLASSCODE"]));
            sb.Append("CK_CODE=" + strSQL(d["CK_CODE"]));
            sb.Append("CK_DESCRIBE=" + strSQL(d["CK_DESCRIBE"]));
            sb.Append("CK_MEASUREMENT=" + strSQL(d["CK_MEASUREMENT"]));
            //sb.Append("CK_Quantity=" + d["CK_Quantity"] + ",");
            sb.Append("CK_TIME=TO_DATE('"+d["CK_TIME"]+ "','yyyy-mm-dd hh24:mi:ss'),");
            sb.Append("CK_COMPANY=" + strSQL(d["CK_COMPANY"]));
            sb.Append("CK_CUSTODIAN=" + strSQL(d["CK_CUSTODIAN"]));
            sb.Append("CK_LOCATION=" + strSQL(d["CK_LOCATION"]));
            sb.Append("CK_LOCATIONNUMBER=" + strSQL(d["CK_LOCATIONNUMBER"]));
            sb.Append("CK_REMARK=" + strSQL(d["CK_REMARK"]));
            string sql = sb.ToString();
            sql = sql.TrimEnd(',');
            sql += " WHERE CK_ID='" + d["CK_ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string strSQL(object t)
        {
            string str = string.Empty;
            if (t != "" && t != null)
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

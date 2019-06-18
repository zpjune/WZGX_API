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
            sb.Append(strSQL(d["CK_ClassCode"]));
            sb.Append(strSQL(d["CK_Code"]));
            sb.Append(strSQL(d["CK_Describe"]));
            sb.Append(strSQL(d["CK_Measurement"]));
            //sb.Append(d["CK_Quantity"] + ",");
            sb.Append(strSQL(d["CK_Time"]));
            sb.Append(strSQL(d["CK_Company"]));
            sb.Append(strSQL(d["CK_Custodian"]));
            sb.Append(strSQL(d["CK_Location"]));
            sb.Append(strSQL(d["CK_LocationNumber"]));
            sb.Append(strSQL(d["CK_Remark"]));
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
            sb.Append("CK_ClassCode=" + strSQL(d["CK_ClassCode"]));
            sb.Append("CK_Code=" + strSQL(d["CK_Code"]));
            sb.Append("CK_Describe=" + strSQL(d["CK_Describe"]));
            sb.Append("CK_Measurement=" + strSQL(d["CK_Measurement"]));
            //sb.Append("CK_Quantity=" + d["CK_Quantity"] + ",");
            sb.Append("CK_Time=" + strSQL(d["CK_Time"]));
            sb.Append("CK_Company=" + strSQL(d["CK_Company"]));
            sb.Append("CK_Custodian=" + strSQL(d["CK_Custodian"]));
            sb.Append("CK_Location=" + strSQL(d["CK_Location"]));
            sb.Append("CK_LocationNumber=" + strSQL(d["CK_LocationNumber"]));
            sb.Append("CK_Remark=" + strSQL(d["CK_Remark"]));
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS
{
    public class DictionaryConfigDB
    {
        DBTool db = new DBTool("");
        public DataTable getData()
        {
            string sql = @"SELECT * FROM TS_DICTIONARY ORDER BY ""CODE"",""SORTNO""";
            return db.GetDataTable(sql);
        }

        public string editNode(Dictionary<string, object> d)
        {
            string sql = @"UPDATE TS_DICTIONARY SET ""PARENTCODE""='" + d["ParentCode"] + "',";
            sql += @"""CODE""='" + d["Code"] + "',";
            sql += @"""NAME""='" + d["Name"] + "',";
            sql += @"""S_UPDATEBY""='" + d["S_UpdateBy"] + "',";
            sql += @"""S_UPDATEDATE""= to_date('" + d["S_UpdateDate"] + "','yyyy-MM-dd HH24:mi:ss')";
            if (d["EnglishCode"] != null && d["EnglishCode"].ToString() != "")
            {
                sql += @",""ENGLISHCODE""='" + d["EnglishCode"] + "'";
            }
            if (d["SortNo"] != null && d["SortNo"].ToString() != "")
            {
                sql += @",""SORTNO""=" + d["SortNo"] + "";
            }
            sql += @" WHERE ""S_ID""='" + d["S_Id"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public string createNode(Dictionary<string, object> d)
        {
            StringBuilder sql = new StringBuilder();
            //string sql = "INSERT INTO tax_dictionary(S_Id,S_CreateDate,S_CreateBy,ParentCode,Code,Name,EnglishCode,SortNo)VALUES(";
            sql.Append(@"INSERT INTO TS_DICTIONARY(""S_Id"",""S_CreateDate"",""S_CreateBy"",""ParentCode"",""Code"",""Name"",""EnglishCode"",""SortNo"")VALUES('".ToUpper());
            sql.Append(d["S_Id"]);
            sql.Append("',");
            sql.Append("to_date('"+d["S_CreateDate"]+ "','yyyy-MM-dd HH24:mi:ss')");
            sql.Append(",'");
            sql.Append(d["S_CreateBy"]);
            sql.Append("',");
            if (d["ParentCode"] == null || d["ParentCode"].ToString() == "")
            {
                sql.Append("null");
            }
            else
            {
                sql.Append("'"+d["ParentCode"]+"'");
            }
            sql.Append(",'");
            sql.Append(d["Code"]);
            sql.Append("','");
            sql.Append(d["Name"]);
            sql.Append("','");
            sql.Append(d["EnglishCode"] == null ? "" : d["EnglishCode"]);
            sql.Append("',");
            sql.Append(d["SortNo"] == null ? 0 : d["SortNo"]);
            sql.Append(")");
            return db.ExecutByStringResult(sql.ToString().Trim());
        }

        public DataTable getRepeatInfo(Dictionary<string, object> d)
        {
            string sql = "SELECT * FROM  TS_DICTIONARY WHERE 1=1";
            sql += " AND 'CODE'='" + d["Code"] + "'";
            //sql+=" OR Name='" + d["Name"] + "'";
            return db.GetDataTable(sql);
        }

        public string delNode(Dictionary<string, object> d)
        {
            string sql = @"DELETE FROM TS_DICTIONARY WHERE ""S_ID""='" + d["S_Id"] + "'";
            return db.ExecutByStringResult(sql);
        }

        public DataTable search(string param)
        {
            string sql = @"SELECT * FROM TS_DICTIONARY WHERE ""CODE""='" + param + "'" + @" OR ""NAME""='" + param + "'" + @" OR ""ENGLISHCODE""='" + param + "'";
            return db.GetDataTable(sql);
        }
        public DataTable GetCodeOptions(string ParentCode)
        {
            string sql = " select CODE,NAME FROM TS_DICTIONARY where PARENTCODE='" + ParentCode + "'";
            return db.GetDataTable(sql);
        }
    }
}

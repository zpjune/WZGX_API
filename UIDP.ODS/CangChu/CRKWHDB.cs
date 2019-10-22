using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class CRKWHDB
    {
        DBTool db = new DBTool("");

        public DataTable GetCRKInfo(string DK_CODE,string ERDATE)
        {
            string sql = " SELECT a.*,b.NAME from WZ_CRKL a " +
                "left join TS_DICTIONARY b ON a.DK_CODE=b.CODE" +
                " where 1=1";
            if (!string.IsNullOrEmpty(DK_CODE))
            {
                sql += " AND DK_CODE='" + DK_CODE + "'";
            }
            if (!string.IsNullOrEmpty(ERDATE))
            {
                //sql += " AND TO_CHAR(ERDATE,'YYYY-MM') = TO_CHAR('" + ERDATE + "'," + "'YYYY-MM')";
                sql += " AND ERDATE='" + ERDATE + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateCRKInfo(Dictionary<string,object> d)
        {
            string sql = " INSERT INTO WZ_CRKL (ID,DK_CODE,ERDATE,RKL,CKL,CREATE_BY,CREATE_DATE) VALUES(";
            sql += GetSqlStr(Guid.NewGuid());
            sql += GetSqlStr(d["DK_CODE"]);
            //sql += "TO_DATE('" + d["ERDATE"] + "','yyyy/MM'),";
            sql += GetSqlStr(d["ERDATE"]);
            sql += GetSqlStr(d["RKL"], 1);
            sql += GetSqlStr(d["CKL"], 1);
            sql += GetSqlStr(d["userid"]);
            sql += "TO_DATE('" + DateTime.Now + "','yyyy/MM/dd HH24:Mi:ss')";
            sql = sql.TrimEnd(',');
            sql += ")";
            return db.ExecutByStringResult(sql);
        }

        public string UpdateCRKInfo(Dictionary<string,object> d)
        {
            string sql = " UPDATE WZ_CRKL SET DK_CODE=";
            sql += GetSqlStr(d["DK_CODE"]);
            //sql += " ERDATE=" + "TO_DATE('" + d["ERDATE"] + "','yyyy/MM'),";
            sql += " ERDATE=" + GetSqlStr(d["ERDATE"]);
            sql += " RKL="+ GetSqlStr(d["RKL"], 1);
            sql += " CKL=" + GetSqlStr(d["CKL"], 1);
            sql = sql.TrimEnd(',');
            sql += " where ID='" + d["ID"] + "'";
            
            return db.ExecutByStringResult(sql);
        }

        public string DelCRKInfo(string ID)
        {
            string sql = "DELETE WZ_CRKL WHERE ID='" + ID + "'";
            return db.ExecutByStringResult(sql);
        }


        public string GetSqlStr(object t,int type=0)
        {
            if (t == null || t.ToString() == "")
            {
                return "NULL,";
            }
            else
            {
                if (type == 0)
                {
                    return "'" + t + "',";
                }
                else
                {
                    return t + ",";
                }
            }
        }
    }
}

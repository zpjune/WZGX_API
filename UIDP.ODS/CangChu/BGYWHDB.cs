using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class BGYWHDB
    {
        DBTool db = new DBTool("");
        public DataTable GetBGYInfo(string WORKER_CODE,string WORKER_NAME,string WORKER_DP)
        {
            string sql = "select * from WZ_BGY a";
            sql += " left join WZ_DW b on a.WORKER_DP=b.DW_CODE where 1=1";
            if (!String.IsNullOrEmpty(WORKER_CODE))
            {
                sql += " AND WORKERCODE='" + WORKER_CODE + "'";
            }
            if (!String.IsNullOrEmpty(WORKER_NAME))
            {
                sql += " AND WORKERNAME='" + WORKER_NAME + "'";
            }
            if (!String.IsNullOrEmpty(WORKER_DP))
            {
                sql += " AND WORKER_DP='" + WORKER_DP + "'";
            }
            return db.GetDataTable(sql);
        }


        public string CreateBGYInfo(Dictionary<string,object> d)
        {
            string sql = "insert into WZ_BGY (WORKER_CODE,WORKER_NAME,WORKER_DP) VALUES('" + d["WORKER_CODE"] + "','" + d["WORKER_NAME"] + "','" + d["WORKER_DP"] + "')";
            return db.ExecutByStringResult(sql);
        }

        public string EditBGYInfo(Dictionary<string,object> d)
        {
            string sql = "update WZ_BGY SET WORKER_NAME='" + d["WORKER_NAME"] + "',WORKER_DP='" + d["WORKER_DP"] + "' WHERE WORKER_CODE='" + d["WORKER_CODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public string DelBGYInfo(Dictionary<string, object> d)
        {
            string sql = "delete from WZ_BGY where WORKER_CODE='" + d["WORKER_CODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public DataTable GetGCInfo()
        {
            string sql = "select * from WZ_DW";
            return db.GetDataTable(sql);
        }
    }
}

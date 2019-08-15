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
        public DataTable GetBGYInfo(string WORKERCODE,string WORKERNAME,string WORKER_DP)
        {
            string sql = "select * from WZ_BGY where 1=1";
            if (!String.IsNullOrEmpty(WORKERCODE))
            {
                sql += " AND WORKERCODE='" + WORKERCODE + "'";
            }
            if (!String.IsNullOrEmpty(WORKERNAME))
            {
                sql += " AND WORKERNAME='" + WORKERNAME + "'";
            }
            if (!String.IsNullOrEmpty(WORKER_DP))
            {
                sql += " AND WORKER_DP='" + WORKER_DP + "'";
            }
            return db.GetDataTable(sql);
        }


        public string CreateBGYInfo(Dictionary<string,object> d)
        {
            string sql = "insert into WZ_BGY (WORKERCODE,WORKERNAME,WORK_DP) VALUES('" + d["WORKERCODE"] + "','" + d["WORKERNAME"] + "','" + d["WORK_DP"] + "')";
            return db.ExecutByStringResult(sql);
        }

        public string EditBGYInfo(Dictionary<string,object> d)
        {
            string sql = "update WZ_BGY SET WORKERNAME='" + d["WORKERNAME"] + "',WORKER_DP='" + d["WORKER_DP"] + "' WHERE WORKERCODE='" + d["WORKERCODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public string DelBGYInfo(Dictionary<string, object> d)
        {
            string sql = "delete from WZ_BGY where WORKERCODE='" + d["WOKERCODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public DataTable GetGCInfo()
        {
            string sql = "select * from WZ_DW";
            return db.GetDataTable(sql);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class GCWHDB
    {
        DBTool db = new DBTool("");

        public DataTable GetGCInfo(string DW_CODE,string DW_NAME)
        {
            string sql = "select * from WZ_DW where 1=1";
            if (!String.IsNullOrEmpty(DW_CODE))
            {
                sql += " AND DW_CODE='" + DW_CODE + "'";
            }
            if (!String.IsNullOrEmpty(DW_NAME))
            {
                sql += " AND DW_NAME='" + DW_NAME + "'";
            }
            return db.GetDataTable(sql);
        }

        public string CreateGCInfo(Dictionary<string,string> d)
        {
            string sql = "insert into WZ_DW(DW_CODE,DW_NAME)VALUES('" + d["DW_CODE"] + "','" + d["DW_NAME"] + "')";
            return db.ExecutByStringResult(sql);
        }

        public string EditGCInfo(Dictionary<string,string> d)
        {
            string sql = " UPDATE WZ_DW SET DW_NAME='" + d["DW_NAME"] + "' WHERE DW_CODE='" + d["DW_CODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
        public string DelGCInfo(Dictionary<string,object> d)
        {
            string sql = "DELETE WZ_DW WHERE DW_CODE='" + d["DW_CODE"] + "'";
            return db.ExecutByStringResult(sql);
        }
    }
}

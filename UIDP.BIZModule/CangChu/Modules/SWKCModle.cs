using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.BIZModule.CangChu.Models;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class SWKCModle
    {
        SWKCDB db = new SWKCDB();
        public Dictionary<string,object> GetFacInfo(string WERKS, string LGORT, string LGORT_NAME,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetFacInfo(WERKS, LGORT, LGORT_NAME);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功！但是没有数据";
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string,object> GetCompositeInfo(string WERKS, string LGORT, string LGORT_NAME, string MATNR, string MAKTX,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetCompositeInfo(WERKS, LGORT, LGORT_NAME, MATNR, MAKTX);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["message"] = "成功！";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功，但是没有数据！";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> GetParentList(string MATKL, int page, int limit, int level)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                List<SWKCNode> list = new List<SWKCNode>();
                DataTable dt = db.GetAllInfo(MATKL, "");
                if (!string.IsNullOrEmpty(MATKL))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SWKCNode model = new SWKCNode();
                        model.ID = Guid.NewGuid();
                        model.Code = dr["MATNR"].ToString();
                        model.Name = dr["MAKTX"].ToString();
                        model.Unit = dr["MEINS"].ToString();
                        model.Number = decimal.Parse(dr["GESME"].ToString());
                        model.Location = dr["LGORT_NAME"].ToString();
                        model.hasChildren = false;
                        list.Add(model);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SWKCNode model = new SWKCNode();
                        model.ID = Guid.NewGuid();
                        model.Code = dr["DLCODE"].ToString();
                        model.Name = dr["DLNAME"].ToString();
                        model.hasChildren = true;
                        list.Add(model);
                    }
                }               
                if (list.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                    r["items"] = list.Skip((page - 1) * limit).Take(limit).ToList();
                    r["totoal"] = list.Count;
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功！,but no info";
                    r["totoal"] = list.Count;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public List<SWKCNode> GetChildrenList(string code,int level)
        {
            return CreateNode(code, level + 1);
        }

        public List<SWKCNode> CreateNode(string code,int level)
        {
            List<SWKCNode> list = new List<SWKCNode>();
            DataTable dt = db.GetAllInfo("", code, level);
            switch (level)
            {
                case 0:
                    foreach (DataRow dr in dt.Rows)
                    {
                        SWKCNode model = new SWKCNode();
                        model.ID = Guid.NewGuid();
                        model.Code = dr["DLCODE"].ToString();
                        model.Name = dr["DLNAME"].ToString();
                        model.hasChildren = true;
                        list.Add(model);
                    }
                    break;
                case 1:
                    foreach (DataRow dr in dt.Rows)
                    {
                        SWKCNode model = new SWKCNode();
                        model.ID = Guid.NewGuid();
                        model.Code = dr["ZLCODE"].ToString();
                        model.Name = dr["ZLNAME"].ToString();
                        model.hasChildren = true;
                        list.Add(model);
                    }
                    break;
                case 2:
                    foreach (DataRow dr in dt.Rows)
                    {
                        SWKCNode model = new SWKCNode();
                        model.ID = Guid.NewGuid();
                        model.Code = dr["XLCODE"].ToString();
                        model.Name = dr["XLNAME"].ToString();
                        model.hasChildren = true;
                        list.Add(model);
                    }
                    break;
                case 3:
                    foreach (DataRow dr in dt.Rows)
                    {
                        SWKCNode model = new SWKCNode();
                        model.ID = Guid.NewGuid();
                        model.Code = dr["PMCODE"].ToString();
                        model.Name = dr["PMNAME"].ToString();
                        model.Unit = dr["MEINS"].ToString();
                        model.Number = decimal.Parse(dr["GESME"].ToString());
                        model.Location = dr["LGORT_NAME"].ToString();
                        model.hasChildren = false;
                        list.Add(model);
                    }
                    break;
            }
            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.BIZModule.CangChu.Models;
using UIDP.ODS.CangChu;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class ZWKCModule
    {
        ZWKCDB db = new ZWKCDB();
        public Dictionary<string, object> GetParentWLZList(string MATKL, string CODE,int level,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            List<ZWKCNode> list = new List<ZWKCNode>();
            try
            {
                DataTable dt = db.GetTotalInfo(MATKL, CODE, level);
                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(MATKL))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ZWKCNode model = new ZWKCNode();
                            model.CODE = dr["DLCODE"].ToString();
                            model.NAME = dr["DLNAME"].ToString();
                            model.SALK3 = decimal.Parse(dr["SALK3"].ToString());
                            model.ID = Guid.NewGuid().ToString();
                            model.level = 0;//根节点level为0
                            model.hasChildren = true;
                            list.Add(model);
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ZWKCNode model = new ZWKCNode();
                            model.CODE = dr["MATKL"].ToString();
                            model.NAME = dr["PMNAME"].ToString();
                            model.SALK3 = decimal.Parse(dr["SALK3"].ToString());
                            model.ID = Guid.NewGuid().ToString();
                            model.level = 3;//最下级子节点
                            model.hasChildren = false;
                            list.Add(model);
                        }
                    }
                    r["code"] = 2000;
                    r["items"] = list.Skip(page-1*limit).Take(limit).ToList();
                    r["total"] = dt.Rows.Count;
                    r["message"] = "success";
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "success,but no info";
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public List<ZWKCNode> GetChildrenList(string CODE,int level)
        {
            try
            {
                DataTable dt = db.GetTotalInfo("", CODE, level + 1);
                List<ZWKCNode> list = new List<ZWKCNode>();
                if (dt.Rows.Count > 0)
                {
                    switch (level)
                    {
                        case 0:
                            foreach(DataRow dr in dt.Rows)
                            {
                                ZWKCNode model = new ZWKCNode();
                                model.CODE = dr["ZLCODE"].ToString();
                                model.NAME = dr["ZLNAME"].ToString();
                                model.SALK3 = decimal.Parse(dr["SALK3"].ToString());
                                model.ID = Guid.NewGuid().ToString();
                                model.level = level+1;//节点等级自加1
                                model.hasChildren = true;
                                list.Add(model);
                            }
                            break;

                        case 1:
                            foreach (DataRow dr in dt.Rows)
                            {
                                ZWKCNode model = new ZWKCNode();
                                model.CODE = dr["XLCODE"].ToString();
                                model.NAME = dr["XLNAME"].ToString();
                                model.SALK3 = decimal.Parse(dr["SALK3"].ToString());
                                model.ID = Guid.NewGuid().ToString();
                                model.level = level+1;//节点等级自加1
                                model.hasChildren = true;
                                list.Add(model);
                            }
                            break;

                        case 2:
                            foreach (DataRow dr in dt.Rows)
                            {
                                ZWKCNode model = new ZWKCNode();
                                model.CODE = dr["MATKL"].ToString();
                                model.NAME = dr["PMNAME"].ToString();
                                model.SALK3 = decimal.Parse(dr["SALK3"].ToString());
                                model.ID = Guid.NewGuid().ToString();
                                model.level = level+1;//节点等级自加1
                                model.hasChildren = false;
                                list.Add(model);
                            }
                            break;
                    }
                }
                return list;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}

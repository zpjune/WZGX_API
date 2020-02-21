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
                            //model.level = 0;//根节点level为0
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
                            //model.level = 3;//最下级子节点
                            model.hasChildren = false;
                            list.Add(model);
                        }
                    }
                    r["code"] = 2000;
                    r["items"] = list.Skip((page-1)*limit).Take(limit).ToList();//使用linq分页,后期可以考虑使用数据库分页
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = 2000;
                    r["items"] = new DataTable();
                    r["message"] = "成功！,但是没有数据";
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
                                //model.level = level+1;//节点等级自加1
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
                                //model.level = level+1;//节点等级自加1
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
                                //model.level = level+1;//节点等级自加1
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
        public Dictionary<string,object> GetFacMoney(string BWKEY, string BWKEY_NAME,int page,int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetFacMoney(BWKEY, BWKEY_NAME);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = 2000;
                    r["items"] = new DataTable();
                    r["message"] = "成功！,但是没有数据";
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        public Dictionary<string, object> GetExportsFacMoney()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetFacMoney(null, null);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = 2000;
                    r["items"] = new DataTable();
                    r["message"] = "成功！,但是没有数据";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> GetCompositeInfo(string BWKEY, int type, string CODE, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.GetCompositeInfo(BWKEY, type, CODE, page, limit);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                    r["items"] = ds.Tables[0];
                    r["total"] = ds.Tables[1].Rows[0]["TOTAL"];
                }
                else
                {
                    r["code"] = 2000;
                    r["items"] = new DataTable();
                    r["message"] = "成功！,但是没有数据";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
    }
}

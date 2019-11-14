using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UIDP.BIZModule.Modules;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu
{
    public class WLZWZModule
    {
        WLZWHDB db = new WLZWHDB();

        public Dictionary<string,object> GetParentWLZList(string WLZCODE, string WLZNAME,int limit,int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetParentWLZList(WLZCODE, WLZNAME);
                if (!String.IsNullOrEmpty(WLZCODE) || !String.IsNullOrEmpty(WLZNAME))
                {
                    if (dt.Rows.Count > 0)
                    {
                        r["code"] = 2000;
                        r["message"] = "成功！";
                        r["items"] = CreateFullNode(dt,page,limit);
                        r["total"] = dt.Rows.Count;
                    }
                    else
                    {
                        r["code"] = 2000;
                        r["message"] = "成功！,但是没有数据";
                        r["items"] = new DataTable();
                        r["total"] = 0;
                    }
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        r["code"] = 2000;
                        r["message"] = "成功！";
                        r["items"] = CreateParentNode(dt, page, limit);
                        r["total"] = dt.Rows.Count;
                    }
                    else
                    {
                        r["code"] = 2000;
                        r["message"] = "成功！,但是没有数据";
                        r["items"] = new DataTable();
                        r["total"] = 0;
                    }
                }            
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = "failed!" + e.Message;
            }
            return r;
        }

        public List<WLZModel> GetChildrenWLZList(string DLCODE, string ZLCODE, string XLCODE,string FlagID,int level)
        { 
            try
            {
                DataTable dt = db.GetChildrenWLZList(DLCODE,ZLCODE,XLCODE, level);
                return CreateChildrenNode(dt,FlagID, level);

            }
            catch (Exception e)
            {
                throw e;
            }           
        }
        public List<WLZModel> CreateFullNode(DataTable dt,int page,int limit)
        {
            List<WLZModel> List = new List<WLZModel>();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                WLZModel wLZ = new WLZModel();
                wLZ.ID = dr["ID"].ToString();
                wLZ.DLCODE = dr["DLCODE"].ToString();
                wLZ.DLNAME = dr["DLNAME"].ToString();
                wLZ.ZLCODE = dr["ZLCODE"].ToString();
                wLZ.ZLNAME = dr["ZLNAME"].ToString();
                wLZ.XLCODE = dr["XLCODE"].ToString();
                wLZ.XLNAME = dr["XLNAME"].ToString();
                wLZ.PMCODE = dr["PMCODE"].ToString();
                wLZ.PMNAME = dr["PMNAME"].ToString();
                wLZ.XHGGGF = dr["XHGGGF"].ToString();
                wLZ.JBJLDW = dr["JBJLDW"].ToString(); 
                wLZ.hasChildren = false;
                wLZ.flagID = "ALL" + i;
                i++;
                List.Add(wLZ);
            }
            return List.Skip((page - 1) * limit).Take(limit).ToList();
        }

        public List<WLZModel> CreateParentNode(DataTable dt,int page,int limit)
        {
            List<WLZModel> ParentList = new List<WLZModel>();
            int i = 0;
            foreach(DataRow dr in dt.Rows)
            {
                WLZModel wLZ = new WLZModel();
                //wLZ.ID = dr["ID"].ToString();
                wLZ.DLCODE = dr["DLCODE"].ToString();
                wLZ.DLNAME = dr["DLNAME"].ToString();
                //wLZ.ZLCODE = dr["ZLCODE"].ToString();
                //wLZ.ZLNAME = dr["ZLNAME"].ToString();
                //wLZ.XLCODE = dr["XLCODE"].ToString();
                //wLZ.XLNAME = dr["XLNAME"].ToString();
                //wLZ.PMCODE = dr["PMCODE"].ToString();
                //wLZ.PMNAME = dr["PMNAME"].ToString();
                //wLZ.XHGGGF = dr["XHGGGF"].ToString();
                //wLZ.JBJLDW = dr["JBJLDW"].ToString();
                //wLZ.SX1MC = dr["SX1MC"].ToString();
                //wLZ.SX1DW = dr["SX1DW"].ToString();
                //wLZ.SX2MC = dr["SX2MC"].ToString();
                //wLZ.SX2DW = dr["SX2DW"].ToString();
                //wLZ.SX3MC = dr["SX3MC"].ToString();
                //wLZ.SX3DW = dr["SX3DW"].ToString();
                //wLZ.SX4MC = dr["SX4MC"].ToString();
                //wLZ.SX4DW = dr["SX4DW"].ToString();
                //wLZ.SX5MC = dr["SX5MC"].ToString();
                //wLZ.SX5DW = dr["SX5DW"].ToString();
                //wLZ.SX6MC = dr["SX6MC"].ToString();
                //wLZ.SX6DW = dr["SX6DW"].ToString();
                //wLZ.SX7MC = dr["SX7MC"].ToString();
                //wLZ.SX7DW = dr["SX7DW"].ToString();
                //wLZ.SX8MC = dr["SX8MC"].ToString();
                //wLZ.SX8DW = dr["SX8DW"].ToString();
                //wLZ.SX9MC = dr["SX9MC"].ToString();
                //wLZ.SX9DW = dr["SX9DW"].ToString();
                //wLZ.SX10MC = dr["SX10MC"].ToString();
                //wLZ.SX10DW = dr["SX10DW"].ToString();
                wLZ.hasChildren = true;
                wLZ.flagID = "DL" + i;
                i++;
                ParentList.Add(wLZ);
            }
            return ParentList.Skip((page - 1) * limit).Take(limit).ToList();
        }

        public List<WLZModel> CreateChildrenNode(DataTable dt,string FlagID,int level)
        {
            List<WLZModel> ChildrenList = new List<WLZModel>();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                WLZModel wLZ = new WLZModel();
                
                wLZ.DLCODE = dr["DLCODE"].ToString();
                wLZ.DLNAME = dr["DLNAME"].ToString();
                wLZ.ZLCODE = dr["ZLCODE"].ToString();
                wLZ.ZLNAME = dr["ZLNAME"].ToString();
                if (level == 0)
                {
                    wLZ.flagID =FlagID+"ZL" + i;
                    wLZ.hasChildren = true;
                }
                if (level == 1)
                {
                    wLZ.XLCODE = dr["XLCODE"].ToString();
                    wLZ.XLNAME = dr["XLNAME"].ToString();
                    wLZ.flagID = FlagID+"XL" + i;
                    wLZ.hasChildren = true;
                }
                if (level == 2)
                {
                    wLZ.ID = dr["ID"].ToString();
                    wLZ.XLCODE = dr["XLCODE"].ToString();
                    wLZ.XLNAME = dr["XLNAME"].ToString();
                    wLZ.PMCODE = dr["PMCODE"].ToString();
                    wLZ.PMNAME = dr["PMNAME"].ToString();
                    wLZ.XHGGGF = dr["XHGGGF"].ToString();
                    wLZ.JBJLDW = dr["JBJLDW"].ToString();
                    wLZ.flagID = FlagID+"PM" + i;
                    wLZ.hasChildren = false;
                }
                i++;
                //wLZ.SX1MC = dr["SX1MC"].ToString();
                //wLZ.SX1DW = dr["SX1DW"].ToString();
                //wLZ.SX2MC = dr["SX2MC"].ToString();
                //wLZ.SX2DW = dr["SX2DW"].ToString();
                //wLZ.SX3MC = dr["SX3MC"].ToString();
                //wLZ.SX3DW = dr["SX3DW"].ToString();
                //wLZ.SX4MC = dr["SX4MC"].ToString();
                //wLZ.SX4DW = dr["SX4DW"].ToString();
                //wLZ.SX5MC = dr["SX5MC"].ToString();
                //wLZ.SX5DW = dr["SX5DW"].ToString();
                //wLZ.SX6MC = dr["SX6MC"].ToString();
                //wLZ.SX6DW = dr["SX6DW"].ToString();
                //wLZ.SX7MC = dr["SX7MC"].ToString();
                //wLZ.SX7DW = dr["SX7DW"].ToString();
                //wLZ.SX8MC = dr["SX8MC"].ToString();
                //wLZ.SX8DW = dr["SX8DW"].ToString();
                //wLZ.SX9MC = dr["SX9MC"].ToString();
                //wLZ.SX9DW = dr["SX9DW"].ToString();
                //wLZ.SX10MC = dr["SX10MC"].ToString();
                //wLZ.SX10DW = dr["SX10DW"].ToString();
                ChildrenList.Add(wLZ);
            }
            return ChildrenList;
        }


        public Dictionary<string,object> editNode(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.editNode(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string,object> getDLOptions()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.getDLOptions();
                if (ds.Tables.Count > 0)
                {
                    r["DLOptions"] = ds.Tables[0];
                    //r["ZLOptions"] = ds.Tables[1];
                    //r["XLOptions"] = ds.Tables[2];
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = "成功！,但是没有数据";
                    r["items"] = new DataTable();
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> getZLOptions(string DLCODE)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable ds = db.getZLOptions(DLCODE);
                if (ds.Rows.Count > 0)
                {
                    r["ZLOptions"] = ds;
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = "成功！,但是没有数据";
                    r["items"] = new DataTable();
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string, object> getXLOptions(string DLCODE,string ZLCODE)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable ds = db.getXLOptions(DLCODE,ZLCODE);
                if (ds.Rows.Count > 0)
                {
                    r["XLOptions"] = ds;
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = "成功！,但是没有数据";
                    r["items"] = new DataTable();
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string,object> delNode(string id)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.delNode(id);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string,object> createNode(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                d["ID"] = Guid.NewGuid();
                string b = db.createNode(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
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

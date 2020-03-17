using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.BIZModule.CangChu.Models;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class BGYModule
    {
        BGYWHDB db = new BGYWHDB();
        public Dictionary<string,object> GetBGYInfo(string WORKER_CODE, string WORKER_NAME, string WORKER_DP,int limit,int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetBGYInfo(WORKER_CODE, WORKER_NAME, WORKER_DP);
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
                    r["message"] = "成功！，但是没有数据";
                    r["items"] = new DataTable();
                    r["total"] = 0;
                }
            }
            catch(Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public Dictionary<string,object> CreateBGYInfo(Dictionary<string,object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateBGYInfo(d);
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

        public Dictionary<string, object> EditBGYInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.EditBGYInfo(d);
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

        public Dictionary<string, object> DelBGYInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DelBGYInfo(d);
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

        public Dictionary<string,object> GetGCInfo()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetGCInfo();       
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                    r["items"] = CreateNode(dt);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功！，但是没有数据";
                    r["items"] = new DataTable();
                    r["total"] = 0;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public List<GCTreeNode> CreateNode(DataTable dt)
        {
            List<GCTreeNode> list = new List<GCTreeNode>();
            GCTreeNode GC1 = new GCTreeNode();
            GC1.DW_CODE = "ParentSSNode";
            GC1.DW_NAME = "上市";
            GCChildrenNode(GC1, dt,"Y");
            list.Add(GC1);
            GCTreeNode GC2 = new GCTreeNode();
            GC2.DW_CODE = "ParentWSSNode";
            GC2.DW_NAME = "未上市";
            GCChildrenNode(GC2, dt,"N");
            list.Add(GC2);
            return list;

        }
        public void GCChildrenNode(GCTreeNode node,DataTable dt,string Flag)
        {
            node.Children = new List<GCTreeNode>();
            foreach (DataRow dr in dt.Select("DW_ISSS='"+Flag+"'"))
            {
                GCTreeNode Children = new GCTreeNode();
                Children.DW_CODE = dr["DW_CODE"].ToString();
                if (dr["DW_NAME"].ToString().Contains("大港油田公司"))
                {
                    Children.DW_NAME = dr["DW_NAME"].ToString().Replace("大港油田公司", "");
                }
                else
                {
                    if (dr["DW_NAME"].ToString().Contains("大港油田"))
                    {
                        Children.DW_NAME = dr["DW_NAME"].ToString().Replace("大港油田", "");
                    }
                    else
                    {
                        Children.DW_NAME = dr["DW_NAME"].ToString();
                    }
                }
                //Children.DW_NAME = dr["DW_NAME"].ToString();

                Children.Children = null;
                node.Children.Add(Children);
            }
        }

        public Dictionary<string, object> GetCKHInfo(string PARENTCODE)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetCKHInfo(PARENTCODE);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功！";
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功！，但是没有数据";
                    r["items"] = new DataTable();
                    r["total"] = 0;
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.BIZModule.CangChu.Models;
using UIDP.BIZModule.Modules;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class ZDWZPZModel
    {
        ZDWZPZDB db = new ZDWZPZDB();

        public Dictionary<string, object> GetZDWZPZInfo(string WLZ_CODE, string WL_CODE, string WL_NAME, int limit, int page)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetZDWZPZInfo(WLZ_CODE, WL_CODE, WL_NAME);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }


        public Dictionary<string, object> CreateZDWZPZInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            d.Add("ID", Guid.NewGuid().ToString());
            try
            {
                DataTable dt = db.GetRepeat(d["WLZ_CODE"].ToString(), d["WL_CODE"].ToString());
                if (dt.Rows.Count > 0)
                {
                    r["code"] = -1;
                    r["message"] = "重复的信息！";
                    return r;
                }
                string b = db.CreateZDWZPZInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "success";
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


        public Dictionary<string, object> DelZDWZPZInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DelZDWZPZInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "success";
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

        public Dictionary<string, object> EditZDWZPZInfo(Dictionary<string, string> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.EditZDWZPZInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "success";
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


        public Dictionary<string, object> GetPMCODE()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetPMCODE();
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = dt;
                    r["message"] = "success";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "success,but no info";
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public List<WLZTreeNode> CreateParentNode()
        {
            try
            {
                DataTable dt = db.GetParentNode();
                List<WLZTreeNode> list = new List<WLZTreeNode>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        WLZTreeNode Node = new WLZTreeNode();
                        Node.Code = dr["DLCODE"].ToString();
                        Node.DLCODE = Node.Code;
                        Node.label = dr["DLNAME"].ToString();
                        //Node.id = Guid.NewGuid().ToString();
                        Node.children = null;
                        Node.hasChildren = true;
                        Node.IsLoading = false;
                        Node.FlagID = "Parent";
                        list.Add(Node);
                    }

                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WLZTreeNode> CreateChildrenNode(string flagID, string DLCODE, string ZLCODE, string XLCODE)
        {
            try
            {
                List<WLZTreeNode> list = new List<WLZTreeNode>();
                DataTable dt = db.GetChildrenNode(flagID, DLCODE, ZLCODE, XLCODE);
                switch (flagID)
                {
                    case "Parent":
                        foreach (DataRow dr in dt.Rows)
                        {
                            WLZTreeNode Node = new WLZTreeNode();
                            Node.DLCODE = DLCODE;
                            Node.ZLCODE = dr["ZLCODE"].ToString();
                            Node.Code = dr["ZLCODE"].ToString();
                            Node.label = dr["ZLNAME"].ToString();
                            //Node.id = Guid.NewGuid().ToString();
                            Node.children = null;
                            Node.hasChildren = true;
                            Node.FlagID = "ZLNode";
                            Node.IsLoading = false;
                            list.Add(Node);
                        }
                        break;
                    case "ZLNode":
                        foreach (DataRow dr in dt.Rows)
                        {
                            WLZTreeNode Node = new WLZTreeNode();
                            Node.DLCODE = DLCODE;
                            Node.ZLCODE = ZLCODE;
                            Node.Code = dr["XLCODE"].ToString();
                            Node.XLCODE = Node.Code;
                            Node.label = dr["XLNAME"].ToString();
                            //Node.id = Guid.NewGuid().ToString();
                            Node.children = null;
                            Node.hasChildren = true;
                            Node.FlagID = "XLNode";
                            Node.IsLoading = false;
                            list.Add(Node);
                        }
                        break;
                    case "XLNode":
                        foreach (DataRow dr in dt.Rows)
                        {
                            WLZTreeNode Node = new WLZTreeNode();
                            Node.DLCODE = DLCODE;
                            Node.ZLCODE = ZLCODE;
                            Node.XLCODE = XLCODE;
                            Node.Code = dr["PMCODE"].ToString();
                            Node.label = dr["PMNAME"].ToString();
                            //Node.id = Guid.NewGuid().ToString();
                            Node.hasChildren = false;
                            Node.FlagID = "PMNode";
                            Node.IsLoading = false;
                            list.Add(Node);
                        }
                        break;
                }
                return list;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public List<WLZTreeNode> GetEditParentNode(string PMCODE)
        {
            try
            {
                DataSet ds = db.GetNode(PMCODE);
                List<WLZTreeNode> list = new List<WLZTreeNode>();
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    WLZTreeNode Node = new WLZTreeNode();
                    Node.Code = dr["DLCODE"].ToString();
                    Node.DLCODE = Node.Code;
                    Node.label = dr["DLNAME"].ToString();
                    Node.IsLoading = true;
                    Node.hasChildren = true;
                    Node.FlagID = "Parent";
                    if (Node.Code == PMCODE.Substring(0, 2))
                    {
                        Node.children = new List<WLZTreeNode>();
                        GetEditChildrenNode(Node, ds, PMCODE, 0);
                    }
                    else
                    {
                        Node.children = null;
                    }  
                    
                    list.Add(Node);
                }
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void GetEditChildrenNode(WLZTreeNode ParentNode, DataSet ds, string PMCODE, int level)
        {
            List<WLZTreeNode> list = new List<WLZTreeNode>();
            foreach (DataRow dr in ds.Tables[level+1].Rows)
            {
                switch (ParentNode.FlagID)
                {
                    case "Parent":
                        WLZTreeNode ZLNode = new WLZTreeNode();
                        ZLNode.DLCODE = ParentNode.DLCODE;
                        ZLNode.ZLCODE = dr["ZLCODE"].ToString();
                        ZLNode.Code = dr["ZLCODE"].ToString();
                        ZLNode.label = dr["ZLNAME"].ToString();
                        ZLNode.hasChildren = true;
                        ZLNode.FlagID = "ZLNode";
                        if (ZLNode.Code == PMCODE.Substring(0, 4))
                        {
                            ZLNode.children = new List<WLZTreeNode>();
                            ZLNode.IsLoading = true;
                            if (level < 2)
                            {
                                GetEditChildrenNode(ZLNode, ds, PMCODE,1);
                            }
                        }
                        else
                        {
                            ZLNode.children = null;
                        }
                        ParentNode.children.Add(ZLNode);
                        break;
                    case "ZLNode":
                        WLZTreeNode XLNode = new WLZTreeNode();
                        XLNode.DLCODE = PMCODE.Substring(0, 2);
                        XLNode.ZLCODE = PMCODE.Substring(0, 4);                       
                        XLNode.Code = dr["XLCODE"].ToString();
                        XLNode.XLCODE = XLNode.Code;
                        XLNode.label = dr["XLNAME"].ToString();
                        XLNode.hasChildren = true;
                        XLNode.FlagID = "XLNode";
                        if (XLNode.Code == PMCODE.Substring(0, 6))
                        {
                            XLNode.children = new List<WLZTreeNode>();
                            XLNode.IsLoading = true;
                            if (level < 2)
                            {
                                GetEditChildrenNode(XLNode, ds, PMCODE, 2);
                            }
                        }
                        else
                        {
                            XLNode.children = null;
                        }
                        ParentNode.children.Add(XLNode);
                        break;
                    case "XLNode":
                        WLZTreeNode PMNode = new WLZTreeNode();
                        PMNode.DLCODE = PMCODE.Substring(0, 2);
                        PMNode.ZLCODE = PMCODE.Substring(0, 4);
                        PMNode.XLCODE = PMCODE.Substring(0, 6);
                        PMNode.Code = dr["PMCODE"].ToString();
                        PMNode.label = dr["PMNAME"].ToString();
                        PMNode.hasChildren = false;
                        PMNode.FlagID = "XLNode";
                        if (PMNode.Code == PMCODE)
                        {
                            PMNode.children = new List<WLZTreeNode>();
                            PMNode.IsLoading = true;
                            if (level < 2)
                            {
                                GetEditChildrenNode(PMNode, ds, PMCODE, 3);
                            }
                        }
                        else
                        {
                            PMNode.children = null;
                        }
                        ParentNode.children.Add(PMNode);
                        break;

                }
            }
        }
    }
}

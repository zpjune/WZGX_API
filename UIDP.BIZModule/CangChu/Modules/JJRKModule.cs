using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class JJRKModule
    {
        JJRKDB db = new JJRKDB();
        /// <summary>
        /// 紧急入库单查询
        /// </summary>
        /// <param name="CODE">单号</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATNX">物料描述</param>
        /// <param name="MATNX">字典表父节点code</param>
        /// <param name="userid">登录人id</param>
        /// <param name="type">查询类型，0为非审批查询，1为审批待办，2为已办</param>
        /// <param name="limit">每页条数</param>
        /// <param name="page">页数</param>
        /// <param name="SortType">排序方向，0为正序，1位倒叙</param>
        /// <param name="GroupType">排序方式，0为申请单位，1为出库原因，2为库存地点，3为单据状态，4为供应商</param>
        /// <returns></returns>
        public Dictionary<string,object> GetRKInfo
            (string CODE, string MATNR, string MATNX, string ParentCode, string userid, int type, int limit, int page, int SortType = 0, int GroupType = 0,string OrgCode = null, string starttime = null, string endtime = null)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetRKInfo(CODE, MATNR, MATNX, ParentCode, userid, type, SortType, GroupType,OrgCode,starttime,endtime);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功!";
                    r["items"] = KVTool.GetPagedTable(dt, page, limit);
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功但是没有数据!";
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
        /// <summary>
        /// 创建紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string,object> CreateJJRKInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CreateJJRKInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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

        /// <summary>
        /// 修改紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> UpdateJJRKInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.UpdateJJRKInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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


        /// <summary>
        /// 删除紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> DelJJRKInfo(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.DelJJRKInfo(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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


        /// <summary>
        /// 发起紧急入库单流程
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> StartProcess(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.StartProcess(d["ID"].ToString());
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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


        /// <summary>
        /// 撤回紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> Recall(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.Recall(d["ID"].ToString());
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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


        /// <summary>
        /// 审批紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> Approval(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.Approval(d);
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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


        /// <summary>
        /// 紧急入库数据从ERP再次录入到系统中时，保管员调用此方法将模型表和入库表中的数据置为无效
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> CancelRK(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.CancelRK(d["ID"].ToString());
                if (b == "")
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
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
        public Dictionary<string, object> GetOrgInfo()
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetOrgInfo();
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功!";
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功但是没有数据!";
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

        public Dictionary<string, object> GetKCDDInfo(string orgCode)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetKCDDInfo(orgCode);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功!";
                    r["items"] = dt;
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功但是没有数据!";
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

        public Dictionary<string,object> BGYUpdate(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.BGYUpdate(d);
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

        public Dictionary<string, object> BGYSendForm(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = db.BGYSendForm(d);
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

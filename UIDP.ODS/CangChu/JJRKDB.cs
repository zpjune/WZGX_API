using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class JJRKDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 紧急入库查询
        /// </summary>
        /// <param name="CODE">单号</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATNX">物料描述</param>
        /// <param name="MATNX">字典表父节点code</param>
        /// <param name="userid">登录人id</param>
        /// <param name="type">查询类型，0为申请查询，1为审批待办，2保管员补充录出,3为领导查询</param>
        /// <param name="SortType">排序方向，0为正序，1位倒叙</param>
        /// <param name="GroupType">排序方式，0为申请单位，1为出库原因，2为库存地点，3为单据状态，4为供应商</param>
        /// <param name="time">日期范围</param>
        /// <param name="OrgCode">部门编码</param>
        /// <returns></returns>
        public DataTable GetRKInfo
            (string CODE, string MATNR, string MATNX, string ParentCode, string userid, int type, int SortType = 0, int GroupType = 0,string OrgCode = null, string starttime = null, string endtime = null)
        {
            string PartOfSqlSort = " ORDER BY a.CODE DESC";
            string sql = " SELECT DISTINCT a.*,(CASE WHEN b.NAME IS NULL THEN Translate(a.REASON USING NCHAR_CS) ELSE b.NAME END)AS NAME," +
                " c.ORG_SHORT_NAME,e.KCDD_NAME,h.USER_NAME from JJRK a " +
                " left join TS_DICTIONARY b on a.REASON=b.CODE AND b.PARENTCODE='" + ParentCode + "'" +
                " join TS_UIDP_ORG c on a.DW_CODE=c.ORG_CODE " +
                " join WZ_KCDD e ON EXISTS( SELECT 1 FROM TS_UIDP_ORG WHERE ORG_CODE = a.DW_CODE AND e.DWCODE=DW_CODE ) AND a.KCDD = e.KCDD_CODE" +
                " JOIN TS_UIDP_ORG_USER f ON a.CREATEBY=f.USER_ID" +
                " JOIN TS_UIDP_ORG g ON f.ORG_ID=g.ORG_ID" +
                " JOIN TS_UIDP_USERINFO h on a.CREATEBY=h.USER_ID" +
                //" left join WZ_KCDD d on a.KCDD=d.KCDD_CODE" +
                " where 1=1";
            //" AND e.DWCODE=(SELECT DW_CODE FROM TS_UIDP_ORG WHERE ORG_CODE=a.DW_CODE)";筛选去重条件，先合并到join中
            //" AND c.ISINVALID=1";
            switch (type)
            {
                case 0:
                    //sql += " AND a.CREATEBY='" + userid + "'";//第一版 只能自己看自己申请的
                    sql += " AND g.ORG_CODE IN " +//第二版 业务员可以查看本部门全部申请单
                        "(SELECT a.ORG_CODE FROM TS_UIDP_ORG a  LEFT JOIN TS_UIDP_ORG_USER b ON a.ORG_ID=b.ORG_ID WHERE b.USER_ID='" + userid + "')";
                    break;
                case 1:
                    sql += " AND a.APPROVAL_STATUS=1";
                    //第一版 只能看申请单位匹配的信息
                    //sql += " AND a.DW_CODE IN (SELECT ORG_CODE FROM TS_UIDP_ORG a LEFT JOIN TS_UIDP_ORG_USER b ON a.ORG_ID=b.ORG_ID " +
                    //    "LEFT JOIN TS_UIDP_USERINFO c ON b.USER_ID=c.USER_ID WHERE c.USER_ID='" + userid + "')";


                    //第二版 通过分配权限控制审批部门人员查看信息，所以只用查询进入流程的数据
                    break;
                case 2:
                    //第一版 已办事项查询
                    //sql += " AND a.DW_CODE IN (SELECT ORG_CODE FROM TS_UIDP_ORG a LEFT JOIN TS_UIDP_ORG_USER b ON a.ORG_ID=b.ORG_ID " +
                    //    "LEFT JOIN TS_UIDP_USERINFO c ON b.USER_ID=c.USER_ID WHERE c.USER_ID='" + userid + "')";


                    //第二版 保管员可以查看自己单位且自己管理的大库内的已通过审批的紧急出库单，限制条件有两个 一个是KCDD_CODE 一个是DW_CODE(JJRK表内的DW_CODE实际上存的是ORG_CODE)
                    sql += " AND a.APPROVAL_STATUS>=2 AND a.APPROVAL_STATUS<>3";//通过审批部门审批
                    sql += " AND a.KCDD IN (SELECT i.KCDD_CODE FROM WZ_KCDD i" +//表单录出的库存地点是对应库存地点表所配置的地区
                        " JOIN WZ_BGY j ON i.DWCODE = j.WORKER_DP" +
                        " JOIN TS_UIDP_USERINFO k ON j.WORKER_CODE = k.USER_CODE" +
                        " WHERE 1=1" +
                        " AND k.USER_ID='" + userid + "')";
                    //sql += " AND a.DW_CODE=( SELECT ORG_CODE l FROM TS_UIDP_ORG l" +//表单申请的单位是保管员所在单位
                    //    " JOIN TS_UIDP_ORG_USER m ON l.ORG_ID = m.ORG_ID" +
                    //    " JOIN TS_UIDP_USERINFO n ON n.USER_ID = m.USER_ID " +
                    //    " WHERE m.USER_ID = '" + userid + "')";
                    //sql += " AND e.CKH IN (SELECT a.CKH FROM WZ_BGY a JOIN TS_UIDP_USERINFO b ON a.WORKER_CODE=b.USER_CODE WHERE b.USER_ID='" + userid + "')";//保管员所管理的大库
                    sql += "AND a.DW_CODE IN (" +//查询管理员所管理的单位
                        " SELECT ORG_CODE FROM TS_UIDP_ORG " +
                        " WHERE DW_CODE IN (" +
                        " SELECT WORKER_DP FROM WZ_BGY a" +
                        " JOIN TS_UIDP_USERINFO b " +
                        " ON a.WORKER_CODE = b.USER_CODE" +
                        " WHERE b.USER_ID = '" + userid + "'))";
                    sql += " AND e.CKH IN (" +//查询当前登录人所管理的大库
                        " SELECT o.CKH FROM WZ_BGY o" +
                        " JOIN TS_UIDP_USERINFO p ON o.WORKER_CODE = p.USER_CODE " +
                        " WHERE p.USER_ID = '" + userid + "'" +
                        " AND o.WORKER_DP = ( SELECT DW_CODE FROM TS_UIDP_ORG WHERE ORG_CODE = a.DW_CODE ))";
                    break;
                case 3:
                    switch (GroupType)
                    {
                        case 0:
                            PartOfSqlSort = " ORDER BY a.CODE ";
                            break;
                        case 1:
                            PartOfSqlSort = " ORDER BY c.ORG_SHORT_NAME";
                            break;
                        case 2:
                            PartOfSqlSort = " ORDER BY a.REASON";
                            break;
                        case 3:
                            //因为KCDD_CODE同一个值可能匹配多个KCDD_NAME，所以在排序时虽然KCDD_CODE相同,但是匹配出的名称缺不同，因为WZ_KCDD表关联时需要KCDD_CODE和DWCODE才能确定唯一值
                            PartOfSqlSort = " ORDER BY e.KCDD_NAME";
                            break;
                        case 4:
                            PartOfSqlSort = " ORDER BY a.APPROVAL_STATUS";
                            break;
                        case 5:
                            PartOfSqlSort = " ORDER BY a.GYS";
                            break;
                        default:
                            break;
                    }
                    if (SortType != 0)
                    {
                        PartOfSqlSort += " DESC";
                    }
                    break;
                default:
                    throw new Exception("错误的参数！");
            }
            if (!string.IsNullOrEmpty(CODE))
            {
                sql += " AND a.CODE='" + CODE + "'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += "AND a.MATNR like '%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATNX))
            {
                sql += "AND a.MATNX like '%" + MATNX + "%'";
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " AND TO_CHAR(a.CREATEDATE, 'yyyyMMdd')>='" + starttime + "'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " AND TO_CHAR(a.CREATEDATE, 'yyyyMMdd')<='" + endtime + "'";
            }
            if (!string.IsNullOrEmpty(OrgCode))
            {
                sql += "AND c.ORG_CODE='" + OrgCode + "'";
            }
            sql = sql + PartOfSqlSort;
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 创建紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string CreateJJRKInfo(Dictionary<string,object> d)
        {
            string CODE = "1-" + DateTime.Now.ToString("yyyyMM");
            string MATNR = d["MATNR"].ToString();
            string NumStr = db.GetDataTable("SELECT (CASE WHEN SUBSTR(MAX(CODE), 9, 4) IS NULL THEN '0' ELSE SUBSTR(MAX(CODE), 9, 4) END) AS MAXNUM FROM JJRK").Rows[0]["MAXNUM"].ToString();
            NumStr = (Convert.ToInt32(NumStr) + 1).ToString();
            while (NumStr.Length < 4)
            {
                NumStr = "0" + NumStr;
            }
            CODE += NumStr;
            while (MATNR.Length < 18)
            {
                MATNR = "0" + MATNR;
            }
            string sql = " INSERT INTO JJRK (ID,CODE,DW_CODE,MATNR,MATNX,MEINS,RKNUMBER,PRICE,TOTALPRICE,RK_TIME,RKNUMBER1,TOTALPRICE1,REASON,ZRDW,ZRR,CLOSE_TIME," +
            "EFFECTIVE_STATUS,APPROVAL_STATUS,CREATEBY,CREATEDATE,GYS,KCDD,MATKL,OTHERREASON)VALUES('";
            sql += Guid.NewGuid() + "',";
            sql += GetSQLStr(CODE);
            sql += GetSQLStr(d["DW_CODE"]);
            sql += GetSQLStr(MATNR);
            sql += GetSQLStr(d["MATNX"]);
            sql += GetSQLStr(d["MEINS"]);
            sql += GetSQLStr(d["RKNUMBER"],2);
            sql += GetSQLStr(d["PRICE"],2);
            sql += GetSQLStr(d["TOTALPRICE"],2);
            sql += GetSQLStr(d["RK_TIME"],1);
            sql += GetSQLStr(d["RKNUMBER1"],2);
            sql += GetSQLStr(d["TOTALPRICE1"],2);
            sql += GetSQLStr(d["REASON"]);
            sql += GetSQLStr(d["ZRDW"]);
            sql += GetSQLStr(d["ZRR"]);
            sql += GetSQLStr(d["CLOSE_TIME"],1);
            sql += GetSQLStr(1,2);//未审批通过的紧急入库单视为无效数据
            sql += GetSQLStr(0,2);
            sql += GetSQLStr(d["userid"]);
            sql += GetSQLStr(DateTime.Now, 1);
            sql += GetSQLStr(d["GYS"]);
            sql += GetSQLStr(d["KCDD"]);
            sql += GetSQLStr(d["MATKL"]);
            sql += GetSQLStr(d["OTHERREASON"]);
            sql = sql.TrimEnd(',');
            sql += ")";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 修改紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string UpdateJJRKInfo(Dictionary<string,object> d)
        {
            string MATNR = d["MATNR"].ToString();
            while (MATNR.Length < 18)
            {
                MATNR = "0" + MATNR;
            }
            string sql = " UPDATE JJRK SET ";
            sql += " DW_CODE=" + GetSQLStr(d["DW_CODE"]);
            sql += " MATNR=" + GetSQLStr(MATNR);
            sql += " MATNX=" + GetSQLStr(d["MATNX"]);
            sql += " MEINS=" + GetSQLStr(d["MEINS"]);
            sql += " RKNUMBER=" + GetSQLStr(d["RKNUMBER"],2);
            sql += " PRICE=" + GetSQLStr(d["PRICE"],2);
            sql += " TOTALPRICE=" + GetSQLStr(d["TOTALPRICE"],2);
            //sql += " RK_TIME=" + GetSQLStr(d["RK_TIME"],1);
            //sql += " RKNUMBER1=" + GetSQLStr(d["RKNUMBER1"],2);
            //sql += " TOTALPRICE1=" + GetSQLStr(d["TOTALPRICE1"],2);
            sql += " REASON=" + GetSQLStr(d["REASON"]);
            //sql += " ZRDW=" + GetSQLStr(d["ZRDW"]);
            //sql += " ZRR=" + GetSQLStr(d["ZRR"]);
            //sql += " CLOSE_TIME=" + GetSQLStr(d["CLOSE_TIME"],1);
            sql += " UPDATEBY=" + GetSQLStr(d["userid"]);
            sql += " UPDATEDATE=" + GetSQLStr(DateTime.Now,1);
            sql += " GYS=" + GetSQLStr(d["GYS"]);
            sql += " KCDD=" + GetSQLStr(d["KCDD"]);
            sql += "MATKL="+ GetSQLStr(d["MATKL"]);
            sql += " OTHERREASON="+GetSQLStr(d["OTHERREASON"]);
            sql = sql.TrimEnd(',');
            sql += " WHERE ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 保管员修改表单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string BGYUpdate(Dictionary<string, object> d)
        {
            string sql = "UPDATE JJRK SET";
            sql += " RKNUMBER1=" + GetSQLStr(d["RKNUMBER1"], 2);
            sql += " TOTALPRICE1=" + GetSQLStr(d["TOTALPRICE1"], 2);
            sql += " LGPLA=" + GetSQLStr(d["LGPLA"]);
            sql += " APPROVAL_STATUS=5,";
            sql += " BGY_ID=" + GetSQLStr(d["userid"]);
            sql += " BGY_DATE=" + GetSQLStr(DateTime.Now, 1);
            sql = sql.TrimEnd(',');
            sql += " WHERE ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }

        /// <summary>
        /// 保管员提交修改的数据
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string BGYSendForm(Dictionary<string,object> d)
        {
            List<string> list = new List<string>();
            string sql = "UPDATE JJRK SET APPROVAL_STATUS=6,";//将入库单状态置为保管员已已提交
            sql += " BGY_ID=" + GetSQLStr(d["userid"]);
            sql += " BGY_DATE=" + GetSQLStr(DateTime.Now, 1);
            sql += " EFFECTIVE_STATUS=0";//将紧急入库单的状态置为有效
            sql += " WHERE ID='" + d["ID"] + "'";
            string sql1 = " INSERT INTO CONVERT_SWKC (WERKS,ZDHTZD,MATNR,MAKTX,MEINS,GESME,LGORT,KCTYPE,LGPLA,ID,MATKL)VALUES(";
            sql1 += "(SELECT DW_CODE FROM TS_UIDP_ORG WHERE ORG_CODE='" + d["DW_CODE"] + "'),";
            sql1 += GetSQLStr(d["CODE"]);
            sql1 += GetSQLStr(d["MATNR"]);
            sql1 += GetSQLStr(d["MATNX"]);
            sql1 += GetSQLStr(d["MEINS"]);
            sql1 += GetSQLStr(d["RKNUMBER1"]);
            sql1 += GetSQLStr(d["KCDD"]);
            sql1 += GetSQLStr(1, 2);
            sql1 += GetSQLStr(d["LGPLA"]);
            sql1 += GetSQLStr(d["ID"]);
            sql1 += GetSQLStr(d["MATKL"]);
            sql1 = sql1.TrimEnd(',');
            sql1 += ")";
            string sql2 = "INSERT INTO CONVERT_ZWKC (BWKEY,MATNR,SALK3,LBKUM,DANJIA,DLDATE,KCTYPE,ID,MATKL)VALUES(";
            sql2 += "(SELECT DW_CODE FROM TS_UIDP_ORG WHERE ORG_CODE='" + d["DW_CODE"] + "'),";
            sql2+= GetSQLStr(d["MATNR"]);
            sql2 += GetSQLStr(d["TOTALPRICE1"],2);
            sql2 += GetSQLStr(d["RKNUMBER1"], 2);
            sql2 += GetSQLStr(Convert.ToDecimal(Convert.ToDecimal(d["TOTALPRICE1"].ToString())/Convert.ToDecimal(d["RKNUMBER1"].ToString())),2);
            sql2 += GetSQLStr(DateTime.Now.ToString("yyyyMMdd"));
            sql2 += GetSQLStr(1, 2);
            sql2 += GetSQLStr(d["ID"]);
            sql2 += GetSQLStr(d["MATKL"]);
            sql2 = sql2.TrimEnd(',');
            sql2 += ")";
            list.Add(sql);
            list.Add(sql1);
            list.Add(sql2);
            return db.Executs(list);
        }
        /// <summary>
        /// 删除紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string DelJJRKInfo(Dictionary<string, object> d)
        {
            string sql = "delete JJRK where ID='" + d["ID"] + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 发起流程
        /// </summary>
        /// <param name="ID">实例ID</param>
        /// <returns></returns>
        public string StartProcess(string ID)
        {
            string sql = " UPDATE JJRK SET APPROVAL_STATUS=1 WHERE ID='" + ID + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 撤回紧急入库单申请
        /// </summary>
        /// <param name="ID">业务主键</param>
        /// <returns></returns>
        public string Recall(string ID)
        {
            string sql = " UPDATE JJRK SET APPROVAL_STATUS=0 WHERE ID='" + ID + "'";
            return db.ExecutByStringResult(sql);
        }

        /// <summary>
        /// 紧急入库单审批
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string Approval(Dictionary<string, object> d)
        {
            List<string> list = new List<string>();
            string sql = " UPDATE JJRK SET SUGGESTION=" + GetSQLStr(d["SUGGESTION"]);
            sql += "APPROVAL_STATUS=" + GetSQLStr(d["APPROVAL_STATUS"], 2);  
            //if (Convert.ToInt32(d["APPROVAL_STATUS"]) == 2)//此状态为审批通过
            //{
            //    sql += " EFFECTIVE_STATUS=1,";//审批通过将紧急入库单状态置为1
            //    string sql1 = " INSERT INTO CONVERT_SWKC (WERKS,ZDHTZD,MATNR,MAKTX,MEINS,GESME,LGORT,KCTYPE,ID)VALUES(";
            //    sql1 += "(SELECT DW_CODE FROM TS_UIDP_ORG WHERE ORG_CODE='" + d["DW_CODE"] + "'),";
            //    sql1 += GetSQLStr(d["CODE"]);
            //    sql1 += GetSQLStr(d["MATNR"]);
            //    sql1 += GetSQLStr(d["MATNX"]);
            //    sql1 += GetSQLStr(d["MEINS"]);
            //    sql1 += GetSQLStr(d["RKNUMBER1"]);
            //    sql1 += GetSQLStr(d["KCDD"]);
            //    sql1 += GetSQLStr(1,2);
            //    sql1 += GetSQLStr(d["ID"]);
            //    sql1 = sql1.TrimEnd(',');
            //    sql1 += ")";
            //    list.Add(sql1);
            //}
            sql += " SP_ID=" + GetSQLStr(d["userid"]);
            sql += " SP_DATE=" + GetSQLStr(DateTime.Now, 1);
            sql = sql.TrimEnd(',');
            sql += " WHERE ID='" + d["ID"] + "'";
            list.Add(sql);
            return db.Executs(list);
        }
        /// <summary>
        /// 紧急入库数据从ERP再次录入到系统中时，保管员调用此方法将模型表和入库表中的数据置为无效
        /// </summary>
        /// <param name="ID">业务主键</param>
        /// <returns></returns>
        public string CancelRK(string ID)
        {
            List<string> list = new List<string>();
            string sql = " UPDATE JJRK SET EFFECTIVE_STATUS=1,APPROVAL_STATUS=4 where ID='" + ID+"'";
            string sql1 = " UPDATE CONVERT_SWKC SET KCTYPE=3 where ID='" + ID + "'";
            string sql2 = " UPDATE CONVERT_ZWKC SET KCTYPE=3 where ID='" + ID + "'";
            list.Add(sql);
            list.Add(sql1);
            list.Add(sql2);
            return db.Executs(list);
        }

        public DataTable GetOrgInfo()
        {
            string sql = " SELECT ORG_CODE,ORG_SHORT_NAME FROM TS_UIDP_ORG";
            return db.GetDataTable(sql);
        }

        public DataTable GetKCDDInfo(string orgCode)
        {
            string sql = " SELECT DISTINCT KCDD_CODE,KCDD_NAME FROM WZ_KCDD where DWCODE=(SELECT DW_CODE FROM TS_UIDP_ORG WHERE ORG_CODE='" + orgCode+"')";
            return db.GetDataTable(sql);
        }

        public DataSet GetWLInfo(string WL_CODE,string WL_NAME,int page,int limit)
        {
            string MainSql = "(select a.MATKL,a.MATNR,a.MAKTX,b.JBJLDW FROM MARA a ";
            MainSql += " join WZ_WLZ b on a.MATKL=b.PMCODE";
            MainSql += " where 1=1";
            if (!string.IsNullOrEmpty(WL_CODE))
            {
                MainSql += " AND MATNR LIKE '%" + WL_CODE + "%'";
            }
            if (!string.IsNullOrEmpty(WL_NAME))
            {
                MainSql += " AND MAKTX LIKE '%" + WL_NAME + "%'";
            }
            
            MainSql += ")t";
            string PartSql = " {0} SELECT {1} FROM {2}";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }

        /// <summary>
        /// 构造SQL字符串通用方法
        /// </summary>
        /// <param name="t"></param>
        /// <param name="type">类型，0为字符串,1为日期类型，其他为数字类型</param>
        /// <returns></returns>
        public string GetSQLStr(object t,int type=0)
        {
            if (t == null || t.ToString() == "")
            {
                return "null,";
            }
            if (type == 0)
            {
                return "'" + t + "',";
            }
            else if(type == 1)
            {
                return "TO_DATE('" + t + "','yyyy-MM-dd hh24:mi:ss'),";
            }
            else
            {
                return t + ",";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class ZXKDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 查询待出库信息
        /// </summary>
        /// <param name="MATNR">物料编码</param>
        /// <param name="info">物料描述</param>
        /// <param name="FacCode">库存代码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        public DataSet GetDRKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {

            string PartSql = " {0} SELECT {1} FROM {2}";
            //string MainSql = "（SELECT a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,SUM(a.MENGE) AS MENGE,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1,f.DW_NAME " +
            string MainSql = "(SELECT CAST(a.ZDHTZD AS NVARCHAR2(100)) AS ZDHTZD," +
                " CAST(a.MATKL AS NVARCHAR2(100)) AS MATKL," +//从个cast开始到cast结束为上面字段，只不过强制转换了类型
                " CAST(a.MATNR AS NVARCHAR2(100)) AS MATNR," +
                " CAST(e.MAKTX AS NVARCHAR2(100)) AS MAKTX," +
                " CAST(b.JBJLDW AS NVARCHAR2(100)) AS JBJLDW," +
                " CAST(SUM( a.MENGE )AS DECIMAL(18,2)) AS MENGE," +
                " CAST(a.WERKS AS NVARCHAR2(100)) AS WERKS," +
                " CAST(SUM( d.GESME )AS DECIMAL(18,2)) AS GESME," +
                " CAST(c.NAME1 AS NVARCHAR2(100)) AS NAME1," +
                " CAST(f.DW_NAME  AS NVARCHAR2(100)) AS DW_NAME	" +
                " FROM ZC10MMDG072 a " +//入库单表名
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " LEFT JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR AND d.KCTYPE<>3" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " JOIN WZ_KCDD g ON a.WERKS=g.DWCODE AND a.LGORT=g.KCDD_CODE" +
                " WHERE a.ZSTATUS='01'" +
                " AND SUBSTR( a.WERKS, 0, 3 ) = 'C27'" +
                " AND a.ZCJRQ > trunc('" + DateTime.Now.ToString("yyyyMMdd") + "'-7)";
            if (!string.IsNullOrEmpty(FacCode))
            {
                MainSql += " AND g.CKH='" + FacCode + "'";
            }
            MainSql += " {0}" +
                " {1}" +
                " GROUP BY a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,a.WERKS,c.NAME1,f.DW_NAME " +
                //以上是入库单表查询出的数据，union all下面的是紧急入库单查询出来的数据
                " UNION ALL " +
                " SELECT" +
                " CAST(a.CODE AS NVARCHAR2(100)) AS ZDHTZD," +//入库单号，紧急入库单自动生成
                " CAST('' AS NVARCHAR2(100)) AS MATKL," +//物料组，紧急入库单不存在此字段
                " CAST(a.MATNR AS NVARCHAR2(100)) AS MATNR," +//物料编码
                " CAST(a.MATNX AS NVARCHAR2(100)) AS MAKTX," +//物料描述
                " CAST(a.MEINS AS NVARCHAR2(100)) AS JBJLDW," +//计量单位
                " CAST(a.RKNUMBER AS DECIMAL(18,2)) AS MENGE," +//待入库数量
                " CAST(b.DW_CODE AS NVARCHAR2(100)) AS WERKS," +//工厂编号
                " CAST(SUM(d.GESME) AS DECIMAL(18,2)) AS GESME," +//库存数量 
                " CAST(a.GYS  AS NVARCHAR2(100)) AS NAME1," +//供应商名称
                " CAST(b.ORG_NAME  AS NVARCHAR2(100)) AS DW_NAME" +//单位名称
                " FROM JJRK a" +//紧急入库单表名
                " JOIN TS_UIDP_ORG b ON a.DW_CODE=b.ORG_CODE" +
                " JOIN WZ_KCDD c ON a.KCDD=c.KCDD_CODE AND EXISTS( SELECT 1 FROM TS_UIDP_ORG WHERE ORG_CODE = a.DW_CODE AND c.DWCODE=DW_CODE )" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR AND d.KCTYPE<>3" +
                " WHERE a.APPROVAL_STATUS = '2'";
            if (!string.IsNullOrEmpty(FacCode))
            {
                MainSql += " AND c.CKH='" + FacCode + "'";
            }
            MainSql+= " {2}" +
                " {3}" +
                " group by a.CODE,a.MATNR,a.MATNX,a.MEINS,a.RKNUMBER,b.DW_CODE,a.GYS,b.ORG_NAME" +
                " order by ZDHTZD DESC)t";        
            //" AND g.CKH='"+FacCode+"'" +


            if (!string.IsNullOrEmpty(MATNR))
            {
                //MainSql += " AND a.MATNR='" + MATNR + "'";
                MainSql = string.Format(MainSql, "AND a.MATNR like'%" + MATNR+"%'", "{0}", "AND a.MATNR like'%" + MATNR + "%'", "{1}");
            }
            else
            {
                MainSql = string.Format(MainSql, "", "{0}", "", "{1}");
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql = string.Format(MainSql, "AND e.MAKTX LIKE'%" + info + "%'", "AND a.MATNX like'%" + info + "%'");
                //MainSql += " AND e.MAKTX like'" + info + "%'";
            }
            else
            {
                MainSql = string.Format(MainSql, "", "");
            }

            //string KCDDSql = "SELECT KCDD_CODE,DWCODE FROM WZ_KCDD WHERE CKH='" + FacCode + "'";
            //DataTable KCDDData = db.GetDataTable(KCDDSql);
            //if (KCDDData.Rows.Count > 0)
            //{
            //    MainSql += " AND(";
            //    foreach (DataRow dr in KCDDData.Rows)
            //    {
            //        MainSql += " (a.WERKS='" + dr["DWCODE"] + "'";
            //        MainSql += "  AND a.LGORT='" + dr["KCDD_CODE"] + "')";
            //        if (!dr.Equals(KCDDData.Rows[KCDDData.Rows.Count - 1]))
            //        {
            //            MainSql += " OR";
            //        }
            //    }
            //    MainSql += ")";
            //}
            //MainSql += " GROUP BY a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,c.ERNAM,a.WERKS,c.NAME1,f.DW_NAME ORDER BY a.ZDHTZD DESC)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }


        public DataSet GetDCKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {
            string PartSql = " {0} SELECT {1} FROM {2}";
            //string MainSql = "（SELECT a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,SUM(a.MENGE) AS MENGE,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1,f.DW_NAME " +
            string MainSql = "(SELECT CAST(a.ZCKTZD AS NVARCHAR2(100)) AS ZCKTZD," +
                " CAST(a.MATKL AS NVARCHAR2(100)) AS MATKL," +//从个cast开始到cast结束为上面字段，只不过强制转换了类型
                " CAST(a.MATNR AS NVARCHAR2(100)) AS MATNR," +
                " CAST(e.MAKTX AS NVARCHAR2(100)) AS MAKTX," +
                " CAST(b.JBJLDW AS NVARCHAR2(100)) AS JBJLDW," +
                " CAST(SUM( a.ZFHSL )AS DECIMAL(18,2)) AS ZFHSL," +
                " CAST(a.WERKS AS NVARCHAR2(100)) AS WERKS," +
                " CAST(SUM( d.GESME )AS DECIMAL(18,2)) AS GESME," +
                " CAST(c.NAME1 AS NVARCHAR2(100)) AS NAME1," +
                " CAST(f.DW_NAME  AS NVARCHAR2(100)) AS DW_NAME	" +
                " FROM ZC10MMDG078 a " +//出库单表名
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " LEFT JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR AND d.KCTYPE<>3" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " JOIN WZ_KCDD g ON a.WERKS=g.DWCODE AND a.LGORT=g.KCDD_CODE" +
                " WHERE a.ZSTATUS='01'" +
                " AND SUBSTR( a.WERKS, 0, 3 ) = 'C27'" +
                " AND a.ZCJRQ > trunc('" + DateTime.Now.ToString("yyyyMMdd") + "'-7)";
            if (!string.IsNullOrEmpty(FacCode))
            {
                MainSql += " AND g.CKH='" + FacCode + "'";
            }
            MainSql += " {0}" +
                " {1}" +
                " GROUP BY a.ZCKTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,a.WERKS,c.NAME1,f.DW_NAME " +
                //以上是入出库单表查询出的数据，union all下面的是紧急出库单查询出来的数据
                " UNION ALL " +
                " SELECT" +
                " CAST(a.CODE AS NVARCHAR2(100)) AS ZCKTZD," +//出库单号，紧急入库单自动生成
                " CAST(''AS NVARCHAR2(100)) AS MATKL," +//物料组，紧急出库单不存在此字段
                " CAST(a.MATNR AS NVARCHAR2(100)) AS MATNR," +//物料编码
                " CAST(a.MATNX AS NVARCHAR2(100)) AS MAKTX," +//物料描述
                " CAST(a.MEINS AS NVARCHAR2(100)) AS JBJLDW," +//计量单位
                " CAST(a.RKNUMBER AS DECIMAL(18,2)) AS ZFHSL," +//待出库数量
                " CAST(b.DW_CODE AS NVARCHAR2(100)) AS WERKS," +//工厂编号
                " CAST(SUM(d.GESME) AS DECIMAL(18,2)) AS GESME," +//库存数量 
                " CAST(a.GYS  AS NVARCHAR2(100)) AS NAME1," +//供应商名称
                " CAST(b.ORG_NAME  AS NVARCHAR2(100)) AS DW_NAME" +//单位名称
                " FROM JJCK a" +//紧急出库单表名
                " JOIN TS_UIDP_ORG b ON a.DW_CODE=b.ORG_CODE" +
                " JOIN WZ_KCDD c ON a.KCDD=c.KCDD_CODE AND EXISTS( SELECT 1 FROM TS_UIDP_ORG WHERE ORG_CODE = a.DW_CODE AND c.DWCODE=DW_CODE )" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR AND d.KCTYPE<>3" +
                " WHERE a.APPROVAL_STATUS = '2'";
            if (!string.IsNullOrEmpty(FacCode))
            {
                MainSql += " AND c.CKH='" + FacCode + "'";
            }
            MainSql += " {2}" +
                " {3}" +
                " group by a.CODE,a.MATNR,a.MATNX,a.MEINS,a.RKNUMBER,b.DW_CODE,a.GYS,b.ORG_NAME" +
                " order by ZCKTZD DESC)t";               
            if (!string.IsNullOrEmpty(MATNR))
            {
                //MainSql += " AND a.MATNR='" + MATNR + "'";
                MainSql = string.Format(MainSql, "AND a.MATNR like'%" + MATNR + "%'", "{0}", "AND a.MATNR like'%" + MATNR + "%'", "{1}");
            }
            else
            {
                MainSql = string.Format(MainSql, "", "{0}", "", "{1}");
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql = string.Format(MainSql, "AND e.MAKTX LIKE'%" + info + "%'", "AND a.MATNX like'%" + info + "%'");
                //MainSql += " AND e.MAKTX like'" + info + "%'";
            }
            else
            {
                MainSql = string.Format(MainSql, "", "");
            }

            //string KCDDSql = "SELECT KCDD_CODE,DWCODE FROM WZ_KCDD WHERE CKH='" + FacCode + "'";
            //DataTable KCDDData = db.GetDataTable(KCDDSql);
            //if (KCDDData.Rows.Count > 0)
            //{
            //    MainSql += " AND(";
            //    foreach (DataRow dr in KCDDData.Rows)
            //    {
            //        MainSql += " (a.WERKS='" + dr["DWCODE"] + "'";
            //        MainSql += "  AND a.LGORT='" + dr["KCDD_CODE"] + "')";
            //        if (!dr.Equals(KCDDData.Rows[KCDDData.Rows.Count - 1]))
            //        {
            //            MainSql += " OR";
            //        }
            //    }
            //    MainSql += ")";
            //}
            //MainSql += " GROUP BY a.ZDHTZD,a.MATKL,a.MATNR,e.MAKTX,b.JBJLDW,c.ERNAM,a.WERKS,c.NAME1,f.DW_NAME ORDER BY a.ZDHTZD DESC)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }
        /// <summary>
        /// 积压物资分库-第一层 按单位分
        /// </summary>
        /// <param name="ISWZ"></param>
        /// <param name="WERKS"></param>
        /// <param name="DKCODE"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public DataTable GetTotalFK_JYWZ(string ISWZ, string WERKS, string DKCODE)
        {
            string sql = " SELECT a.WERKS,a.WERKS_NAME,COUNT(distinct a.MATNR) AS SL,SUM( a.GESME*NVL(b.DANJIA,0) ) AS SALK3 FROM CONVERT_SWKC a" +
                " LEFT JOIN CONVERT_ZWKC b ON a.WERKS=b.BWKEY AND a.MATNR=b.MATNR" +
                " where months_between(sysdate,to_date(a.ERDAT,'yyyy-mm-dd'))>12" +
                " AND SUBSTR(a.LGPLA, 0, 2)='" + DKCODE + "'" +
                " AND a.KCTYPE=0";
            if (ISWZ == "1")
            {
                sql += "  and substr(a.WERKS,1,3)='C27' ";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += "  and a.WERKS='"+WERKS+"'";
            }
            sql += " GROUP BY a.WERKS,a.WERKS_NAME" +
                " ORDER BY a.WERKS";
            return db.GetDataTable(sql);
        }


        /// <summary>
        /// 积压物资第二层 按大类分类
        /// </summary>
        /// <param name="ISWZ"></param>
        /// <param name="WERKS"></param>
        /// <param name="DKCODE"></param>
        /// <returns></returns>
        public DataTable GetDLFK_JYWZ(string ISWZ,string WERKS,string DKCODE)
        {
            string sql = " select SUBSTR(a.MATKL, 0,2)AS DLCODE,COUNT(distinct a.MATNR) AS SL,SUM(a.GESME) AS GESME,a.MEINS,SUM( a.GESME*NVL(b.DANJIA,0) )AS SALK3" +
                 " FROM CONVERT_SWKC a" +
                 " LEFT JOIN CONVERT_ZWKC b ON a.WERKS=b.BWKEY AND a.MATNR=b.MATNR" +
                 " WHERE SUBSTR(a.LGPLA, 0, 2)='" + DKCODE + "'" +
                 " and a.WERKS='" + WERKS + "'" +
                 " AND months_between(SYSDATE,to_date( a.ERDAT, 'yyyy-mm-dd' )) > 12 " +
                 " AND a.KCTYPE=0";
            if (ISWZ == "1")
            {
                sql += "  and substr(a.WERKS,1,3)='C27' ";
            }
            sql += " GROUP BY SUBSTR(a.MATKL, 0,2),a.MEINS" +
                " ORDER BY SUBSTR(a.MATKL, 0,2)";
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE">大库编码</param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public DataTable GetFK_JYWZ(string DLCODE,string ISWZ,string MEINS,string WERKS, string DKCODE, string MATNR, string MATKL)
        {
            string sql = @" select row_number()over(order by a.werks,a.matnr asc),sum(a.GESME) GESME,a.WERKS,a.WERKS_NAME,a.LGORT_NAME,a.LGORT,MAX(a.MATKL)MATKL,MAX(a.MAKTX)MAKTX,a.ZSTATUS,MAX(a.MEINS)MEINS,
                            '积压' ZT
                               ,a.werks,a.matnr,a.lgort,a.GESME*NVL(b.DANJIA,0)AS SALK3
                            from CONVERT_SWKC a ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += " LEFT JOIN CONVERT_ZWKC b ON a.WERKS = b.BWKEY AND a.MATNR = b.MATNR" +
                " where months_between(sysdate,to_date(a.ERDAT,'yyyy-mm-dd'))>12 " +
                " AND SUBSTR(a.MATKL,0,2)='"+DLCODE+"'" +
                " AND SUBSTR(a.LGPLA,0,2)='"+DKCODE+"'" +
                " AND a.MEINS='"+MEINS+"'";
            if (ISWZ == "1")
            {
                sql += "  and substr(a.WERKS,1,3)='C27' ";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " and  a.WERKS ='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  a.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  a.MATKL like'%" + MATKL + "%'";
            }
            sql += "group by a.werks,a.matnr,a.lgort,a.zstatus,a.WERKS_NAME,a.LGORT_NAME,a.GESME * NVL( b.DANJIA, 0 ) ";//
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 查询平面图仓位状态
        /// </summary>
        /// <param name="FacCode">大库编码</param>
        /// <param name="Month_between">积压月份上限，默认为6</param>
        /// <returns></returns>
        public DataTable GetFacStatus(string FacCode, int Month_between = 12)
        {
            //查询是否积压sql，后续查询用union all 拼上
            string sql = " SELECT SUBSTR(LGPLA,3,2) AS LG,01 AS Status FROM CONVERT_SWKC a" +
                " where ZSTATUS='04'" +
                " AND SUBSTR(LGPLA,0,2)='" + FacCode + "'" +
                " AND MONTHS_BETWEEN(TO_DATE('" + DateTime.Now.ToString("yyyyMMdd") + "','yyyyMMdd'),TO_DATE(ERDAT,'yyyyMMdd'))>" + Month_between +
                " group by SUBSTR(LGPLA,3,2)" +
                " order by SUBSTR(LGPLA,3,2)";
            return db.GetDataTable(sql);
        }
        ///重点物资储备-分库(赞停用)
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCB(string DKCODE, string WERKS_NAME,string MATNR, string MATKL)
        {
            string sql = @" select sum(A.GESME) GESME,A.WERKS,A.WERKS_NAME,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS 
                        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where A.KCTYPE<>3 AND C.CKH='" + DKCODE + "'";
            if (!string.IsNullOrEmpty(WERKS_NAME))
            {
                sql += " and  A.WERKS_NAME like'%" + WERKS_NAME + "%'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            sql += "  group by A.WERKS,A.MATNR,A.WERKS_NAME,B.WL_SORT GROUP BY B.WL_SORT";//
            return db.GetDataTable(sql);
        }
        ///重点物资储备-分库
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getDetailZDWZCBTOTAL(string DKCODE, string MATNR,string MATKL,string MAKTX)
        {
            string sql = @" select sum(A.GESME) GESME,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS 
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where A.KCTYPE<>3 AND  C.CKH='" + DKCODE + "'  and substr(WERKS,1,3)='C27'";
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            if (MAKTX == "钻井泥浆材料")
            {
                sql += " AND (a.MAKTX LIKE '%封闭剂%' " +
                    " or a.MAKTX LIKE '%膨润土粉%' " +
                    " or a.MAKTX LIKE '%片碱%'" +
                    " or a.MAKTX LIKE '%纯碱%'  " +
                    " or a.MAKTX LIKE '%堵漏剂%' " +
                    " or a.MAKTX LIKE '%润滑剂%')";
            }
            else
            {
                sql += " AND a.MAKTX LIKE '%" + MAKTX + "%'";
            }
            sql += "  group by A.MATNR,B.WL_SORT ORDER BY B.WL_SORT";//
            return db.GetDataTable(sql);
        }
        ///重点物资储备-分库明细
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getDetailZDWZCBTOTALDETAIL(string WERKS,string DKCODE, string WERKS_NAME, string MATNR, string MATKL)
        {
            string sql = @" select sum(A.GESME) GESME,A.WERKS,A.WERKS_NAME,A.ZSTATUS,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS 
                        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where A.KCTYPE<>3 AND C.CKH='" + DKCODE + "'  and substr(WERKS,1,3)='C27' ";
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " and  A.WERKS ='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(WERKS_NAME))
            {
                sql += " and  A.WERKS_NAME like'%" + WERKS_NAME + "%'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            sql += "  group by A.WERKS,A.MATNR,A.WERKS_NAME，A.ZSTATUS ";//
            return db.GetDataTable(sql);
        }
        ///重点物资出入库查询-分库
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataSet getZDWZCRK(string DKCODE, string year, string MATNR)
        {

            Dictionary<string, string> dc = new Dictionary<string, string>();
            string sql = "select SUM(MAXHAVING)MAXHAVING,SUM(MINHAVING)MINHAVING";
            sql += " FROM WZ_ZDWZWH WHERE WL_CODE = '" + MATNR + "'";
            sql += "  GROUP BY WL_CODE ";
            dc.Add("zgcb", sql);
            sql = "  select SUM(A.GESME)GESME";
            sql += " FROM CONVERT_SWKC_RECORD A WHERE substr(A.LGPLA,0,2)='"+DKCODE+"' and A.MATNR = '" + MATNR + "'  and A.KCTYPE<>3 AND substr(A.WERKS,0,3)= 'C27' and substr(A.DLDATE,0,4)='" + year + "'";
            sql += "  group by A.DLDATE ";
            dc.Add("kc", sql);
            sql = "select sum(ZSJDHSL)ZSJDHSL,to_char(to_date(B.ERDAT,'yyyy-mm-dd'),'ww')WEEK";
            sql += " from ZC10MMDG072 A";
            sql += " join ZC10MMDG085A B on A.ZDHTZD = B.ZDHTZD AND A.ZITEM = B.ZITEM";
            sql += " JOIN WZ_KCDD C ON C.DWCODE=A.WERKS AND C.KCDD_CODE=A.LGORT AND C.CKH='"+DKCODE+"' ";
            sql += " WHERE A.ZSTATUS > '03' and substr(A.ZCJRQ,1,4)= '" + year + "' and A.MATNR = '" + MATNR + "' AND substr(A.WERKS,0,3)= 'C27'";
            sql += " group by   to_char(to_date(B.ERDAT, 'yyyy-mm-dd'), 'ww')";
            dc.Add("rk", sql);
            sql = " select sum(ZSJFHSL)ZSJFHSL,to_char(to_date(B.ERDAT,'yyyy-mm-dd'),'ww')WEEK";
            sql += "   from ZC10MMDG078 A";
            sql += "   join ZC10MMDG085A B on A.ZCKTZD = B.ZCKTZD AND A.ZCITEM = B.ZCITEM";
            sql += " JOIN WZ_KCDD C ON C.DWCODE=A.WERKS AND C.KCDD_CODE=A.LGORT AND C.CKH='" + DKCODE + "' ";
            sql += "   WHERE A.ZSTATUS > '02' and substr(A.ZCJRQ,1,4)= '" + year + "' and A.MATNR = '" + MATNR + "' AND substr(A.WERKS,0,3)= 'C27'";
            sql += "   group by   to_char(to_date(B.ERDAT, 'yyyy-mm-dd'), 'ww')";
            dc.Add("ck", sql);
            return db.GetDataSet(dc);


            //            string sql = @" 
            //                        select G.*,H.RKSL,I.RKSUMSL,J.CKSL,K.CKSUMSL,F.WL_CODE,'" + yearmonth + "' MONTH ";
            //            sql += @"           from WZ_ZDWZPZ F
            //                         LEFT JOIN (
            //                        select sum(A.GESME) GESME，
            //                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
            //                        from CONVERT_SWKC A
            //                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR";
            //            sql += "           join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS AND A.KCTYPE<>3 AND  C.CKH='" + DKCODE + "'";

            //            sql += @"        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
            //                         group by A.MATNR) G ON F.WL_CODE=G.MATNR
            //                        LEFT JOIN 
            //                        (
            //                        select SUM(A.ZDHSL) RKSL,A.MATNR
            //                        from ZC10MMDG072 A 
            //                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //            sql += "     WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            //            sql += @"         GROUP BY A.MATNR) H ON F.WL_CODE=H.MATNR

            //                        LEFT JOIN 
            //                        (
            //                        select SUM(A.ZDHSL) RKSUMSL ,A.MATNR
            //                        from ZC10MMDG072 A 
            //                       JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //            sql += "           join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS AND C.CKH='" + DKCODE + "'";
            //            sql += "        WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            //            sql += @"       GROUP BY A.MATNR) I ON F.WL_CODE=I.MATNR
            //                        LEFT JOIN 
            //                        (
            //                        select SUM(A.ZFHSL) CKSL,A.MATNR
            //                        from ZC10MMDG078 A ";
            //sql += "           join WZ_KCDD C ON C.KCDD_CODE = A.LGORT AND C.DWCODE = A.WERKS AND C.CKH = '" + DKCODE + "'";
            //            sql += "           JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //            sql += "             WHERE A.ZSTATUS>'03' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            //            sql += @"         GROUP BY A.MATNR) J ON F.WL_CODE=J.MATNR

            //                        LEFT JOIN 
            //                        (
            //                        select SUM(A.ZFHSL) CKSUMSL ,A.MATNR
            //                        from ZC10MMDG078 A 
            //                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //            sql += "           join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS AND  C.CKH='" + DKCODE + "'";
            //            sql += "          WHERE A.ZSTATUS>'03' and substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            //            sql += "         GROUP BY A.MATNR) K ON F.WL_CODE=K.MATNR ";

            //            if (!string.IsNullOrEmpty(MATNR))
            //            {
            //                sql += " where  F.MATNR like'%" + MATNR + "%'";
            //            }

            //            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 重点物资出入库明细-分库
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MONTH"></param>
        /// <returns></returns>
        public DataTable getZDWZCRKDetail(string DKCODE, string MATNR, string MONTH)
        {
            string sql = @" select MAX(CASE WHEN  C.DW_NAME IS NULL THEN B.WEMPF ELSE  C.DW_NAME  END)  WERKS_NAME,SUM(A.ZFHSL) SL
                        from ZC10MMDG078 A
                        JOIN MSEG B ON A.MBLNR=B.MBLNR AND A.ZEILE=B.ZEILE";
            sql += "     join WZ_KCDD C ON C.KCDD_CODE = A.LGORT AND C.DWCODE = A.WERKS AND C.CKH = '" + DKCODE + "'";
            sql += "      LEFT JOIN WZ_DW C ON C.DW_CODE=B.WEMPF";
            sql += "     where A.MATNR='" + MATNR + "'  and  substr(A.ZCJRQ,1,4)=substr('" + MONTH + "',1,4) and substr(A.ZCJRQ,5,2)<=substr('" + MONTH + "',5,2)  group by B.WEMPF";
            return db.GetDataTable(sql);
        }

        public DataSet GetStatusDetail(string LGPLA,string MATNR,string WERKS,int page,int limit)
        {
            string PartSql = " {0} SELECT {1} FROM {2}";
            string date = DateTime.Now.ToString("yyyyMMdd");
            string MainSql = " (SELECT ZSTATUS,WERKS,MATKL,MATNR,MAKTX,MEINS,SUM(GESME) AS GESME," +
                "( CASE WHEN MONTHS_BETWEEN( TO_DATE( '" + date + " ','yyyyMMdd' ), TO_DATE( ERDAT, 'yyyyMMdd' )) > 12 THEN 01 ELSE 02 END ) AS status " +
                " FROM CONVERT_SWKC" +
                " WHERE LGPLA='" + LGPLA + "'" +
                " AND SUBSTR(WERKS,0,3)='C27'";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND MATNR LIKE'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                MainSql += " AND WERKS='" + WERKS + "'";
            }
            MainSql += " group by  ZSTATUS,WERKS,MATKL,MATNR,MAKTX,MEINS," +
                " ( CASE WHEN MONTHS_BETWEEN( TO_DATE( '" + date + "','yyyyMMdd' ), TO_DATE( ERDAT, 'yyyyMMdd' )) > 12 THEN 01 ELSE 02 END )" +
                "  order by ZSTATUS)t";

            string MainTotal = " (SELECT ZSTATUS,WERKS,MATKL,MATNR,MAKTX,MEINS,SUM(GESME) AS GESME" +
                " FROM CONVERT_SWKC" +
                " WHERE LGPLA='" + LGPLA + "'" +
                " AND SUBSTR(WERKS,0,3)='C27'";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainTotal += " AND MATNR LIKE'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                MainTotal += " AND WERKS='" + WERKS + "'";
            }
            MainTotal += " group by  ZSTATUS,WERKS,MATKL,MATNR,MAKTX,MEINS order by ZSTATUS)t";

            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(*) AS TOTAL", MainTotal);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
            //return db.GetDataTable(sql);
        }


        /// <summary>
        /// 获取悬浮窗第一个table
        /// </summary>
        /// <param name="LGPLA"></param>
        /// <returns></returns>
        public DataTable GetFloatWindowFirstInfo(string LGPLA)
        {
            string sql = " SELECT * FROM(" +
                " SELECT b.DLNAME,t.* FROM(" +
                " SELECT SUBSTR( MATKL, 0, 2 ) AS DL,SUM( GESME ) AS GESME,MAX( MEINS ) AS MEINS,COUNT( distinct MATNR ) AS SL FROM CONVERT_SWKC " +
                " WHERE LGPLA = '" + LGPLA + "'" +
                " AND SUBSTR( WERKS, 0, 3 ) = 'C27'" +
                " AND KCTYPE = 0 " +
                " GROUP BY SUBSTR( MATKL, 0, 2 ) ) t" +
                " JOIN WZ_WLZ b ON b.DLCODE = t.DL " +
                " ORDER BY GESME  DESC ) tt" +
                " WHERE ROWNUM=1";
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 获取悬浮窗第二级table
        /// </summary>
        /// <param name="LGPLA"></param>
        /// <returns></returns>
        public DataTable GetFloatWindowInfo(string LGPLA)
        {
            string sql = " select  SUBSTR(MATKL, 0, 2) as DL,SUM(GESME)AS GESME,MAX(MEINS) AS MEINS,COUNT(distinct MATNR) AS SL" +
                " from CONVERT_SWKC  where LGPLA='" + LGPLA + "'" +
                " AND SUBSTR(WERKS,0,3)='C27'" +
                " AND KCTYPE=0 " +
                " group by SUBSTR(MATKL, 0, 2) order by SUBSTR(MATKL, 0, 2) ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取悬浮窗第三级table
        /// </summary>
        /// <param name="LGPLA"></param>
        /// <param name="DLCODE"></param>
        /// <param name="LGORT"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public DataTable GetGetFloatWindowDetailInfo(string LGPLA, string DLCODE, string LGORT, string MATNR, string MATKL)
        {
            string sql = " select ZSTATUS,MATNR,WERKS,WERKS_NAME,MATKL,MATNR,MAKTX,MEINS,SUM(GESME)AS GESME,LGORT,LGORT_NAME FROM CONVERT_SWKC " +
                " where LGPLA='" + LGPLA + "'" +
                " AND SUBSTR(MATKL,0,2)='" + DLCODE + "'";
            if (!string.IsNullOrEmpty(LGORT))
            {
                sql += " AND WERKS='" + LGORT + "'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " AND MATKL='" + MATKL + "'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " AND MATNR like'%" + MATNR + "%'";
            }
            sql += " group by ZSTATUS,MATNR,WERKS,WERKS_NAME,MATKL,MATNR,MAKTX,MEINS,LGORT,LGORT_NAME";
            return db.GetDataTable(sql);
        }


        public DataSet GetJYInfo(string FacCode,string MATKL,string MATNR,int limit,int page)
        {
            string MainSql = " SELECT MATKL,MATNR,WERKS,MAKTX,MEINS,SUM( GESME ) AS GESME,ZSTATUS" +
                " FROM CONVERT_SWKC" +
                " WHERE KCTYPE = 0" +
                " AND SUBSTR(LGPLA,2,2) = '" + FacCode + "'" +
                " {0}" +
                " {1}";//占位符，为后面条件做准备
            MainSql += " GROUP BY MATKL,MATNR,WERKS,MAKTX,MEINS,ZSTATUS";
            
            string ComparisonSql = " SELECT a.WERKS,a.MATNR,a.MATKL,a.ZDHSL,a.LGORT,c.BUDAT_MKPF FROM ZC10MMDG072 a " +
                " JOIN ZC10MMDG085B b ON a.ZDHTZD=b.ZDHTZD AND a.ZITEM=b.ZITEM" +
                " JOIN MSEG c ON a.MBLNR=c.MBLNR AND a.ZEILE=c.ZEILE" +
                " WHERE SUBSTR(b.LGPLA,7,2)='" + FacCode + "'" +
                " {0}" +
                " {1}" +
                " ORDER BY BUDAT_MKPF DESC";
            if (!string.IsNullOrEmpty(MATKL))
            {
                MainSql = string.Format(MainSql, " AND MATKL='" + MATKL + "'", "{0}");
                ComparisonSql = string.Format(ComparisonSql, " AND a.MATKL='" + MATKL + "'", "{0}");
            }
            else
            {
                MainSql = string.Format(MainSql, "", "{0}");
                ComparisonSql = string.Format(ComparisonSql, "", "{0}");
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql = string.Format(MainSql, " AND MATNR like'%" + MATNR + "%'");
                ComparisonSql = string.Format(ComparisonSql, " AND a.MATNR like'%" + MATNR + "%'");
            }
            else
            {
                MainSql = string.Format(MainSql, "");
                ComparisonSql = string.Format(ComparisonSql, "");
            }
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", MainSql);
            list.Add("ComparisonSql", ComparisonSql);
            return db.GetDataSet(list);
        }

        public DataTable GetWLCount(string DKCODE)
        {
            List<string> list = new List<string>()
            {
                "套管",
                "油管",
                "重晶石粉",
                "水泥",
                "支撑剂",
                "钻井泥浆材料",
                "柴油"
            };
            string Total = "";
            string sql = " SELECT NVL(SUM(a.GESME),0) AS GESME," +
                " ( CASE WHEN MAX( a.MEINS ) IS NULL THEN '吨' ELSE MAX( a.MEINS ) END ) AS DW  " +
                " FROM CONVERT_SWKC a" +
                " JOIN WZ_ZDWZPZ b ON B.WL_CODE = a.MATNR" +
                " JOIN WZ_KCDD c ON C.KCDD_CODE = a.LGORT" +
                " AND c.DWCODE = a.WERKS";
            if (!string.IsNullOrEmpty(DKCODE))
            {
                sql += " WHERE c.CKH='" + DKCODE + "'";
            }
            else
            {
                sql += " WHERE c.CKH IS NOT NULL";
            }
            sql += " AND (a.MAKTX LIKE  {0}";
            foreach (string Name in list)
            {
                if(Name.Trim()== "钻井泥浆材料")
                {
                    Total += string.Format(sql, "'%封闭剂%' " +
                    " or a.MAKTX LIKE '%膨润土粉%' " +
                    " or a.MAKTX LIKE '%片碱%'" +
                    " or a.MAKTX LIKE '%纯碱%'  " +
                    " or a.MAKTX LIKE '%堵漏剂%' " +
                    " or a.MAKTX LIKE '%润滑剂%') UNION ALL ");
                }
                else
                {
                    Total += string.Format(sql, "'%" + Name + "%' UNION ALL ");
                }
                
            }
            char[] arr = { 'U', 'N', 'I', 'O', 'N', ' ', 'A', 'L', 'L' };
            Total = Total.TrimEnd(arr);
            return db.GetDataTable(Total);
        }
    }
}

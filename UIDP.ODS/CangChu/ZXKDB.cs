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
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR AND d.KCTYPE<>3" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " LEFT JOIN WZ_KCDD g ON a.WERKS=g.DWCODE AND a.LGORT=g.KCDD_CODE" +
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
                " GROUP BY a.CODE,a.MATNR,a.MATNX,a.MEINS,a.RKNUMBER,b.DW_CODE,a.GYS,b.ORG_NAME" +
                " ORDER BY ZDHTZD DESC)t";        
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
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATNR=d.MATNR AND d.KCTYPE<>3" +
                " JOIN MARA e ON  a.MATNR=e.MATNR" +
                " JOIN WZ_DW f ON a.WERKS=f.DW_CODE" +
                " LEFT JOIN WZ_KCDD g ON a.WERKS=g.DWCODE AND a.LGORT=g.KCDD_CODE" +
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
                " GROUP BY a.CODE,a.MATNR,a.MATNX,a.MEINS,a.RKNUMBER,b.DW_CODE,a.GYS,b.ORG_NAME" +
                " ORDER BY ZCKTZD DESC)t";               
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
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE">大库编码</param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public DataTable GetFK_JYWZ(string ISWZ, string WERKS, string DKCODE, string MATNR, string MATKL)
        {
            string sql = @" select row_number()over(order by werks,matnr asc),sum(GESME) GESME,WERKS,WERKS_NAME,LGORT_NAME,LGORT,MAX(MATKL)MATKL,MAX(MAKTX)MAKTX,ZSTATUS,MAX(MEINS)MEINS,
                            '积压' ZT
                               ,werks,matnr,lgort 
                            from CONVERT_SWKC  ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += "where months_between(sysdate,to_date(ERDAT,'yyyy-mm-dd'))>12 AND substr(LGPLA,1,2)='" + DKCODE + "' ";
            if (ISWZ == "1")
            {
                sql += "  and substr(WERKS,1,3)='C27' ";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " and  WERKS ='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  MATKL like'%" + MATKL + "%'";
            }
            sql += "group by werks,matnr,lgort,zstatus,WERKS_NAME,LGORT_NAME ";//
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
                " GROUP BY SUBSTR(LGPLA,3,2)" +
                " ORDER BY SUBSTR(LGPLA,3,2)";
            return db.GetDataTable(sql);
        }
        ///重点物资储备-分库(赞停用)
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCB(string DKCODE, string WERKS_NAME, string MATNR, string MATKL)
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
            sql += "  group by A.WERKS,A.MATNR,A.WERKS_NAME ";//
            return db.GetDataTable(sql);
        }
        ///重点物资储备-分库
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getDetailZDWZCBTOTAL(string DKCODE, string MATNR, string MATKL)
        {
            string sql = @" select sum(A.GESME) GESME,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS 
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where A.KCTYPE<>3 AND  C.CKH='" + DKCODE + "'  and substr(WERKS,1,3)='C27'  ";
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            sql += "  group by A.MATNR ";//
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
            sql += "  GROUP BY A.DLDATE ";
            dc.Add("kc", sql);
            sql = "select sum(ZSJDHSL)ZSJDHSL,to_char(to_date(B.ERDAT,'yyyy-mm-dd'),'ww')WEEK";
            sql += " from ZC10MMDG072 A";
            sql += " join ZC10MMDG085A B on A.ZDHTZD = B.ZDHTZD AND A.ZITEM = B.ZITEM";
            sql += " JOIN WZ_KCDD C ON C.DWCODE=A.WERKS AND C.KCDD_CODE=A.LGORT AND C.CKH='"+DKCODE+"' ";
            sql += " WHERE A.ZSTATUS > '03' and substr(A.ZCJRQ,1,4)= '" + year + "' and A.MATNR = '" + MATNR + "' AND substr(A.WERKS,0,3)= 'C27'";
            sql += " GROUP BY   to_char(to_date(B.ERDAT, 'yyyy-mm-dd'), 'ww')";
            dc.Add("rk", sql);
            sql = " select sum(ZSJFHSL)ZSJFHSL,to_char(to_date(B.ERDAT,'yyyy-mm-dd'),'ww')WEEK";
            sql += "   from ZC10MMDG078 A";
            sql += "   join ZC10MMDG085A B on A.ZCKTZD = B.ZCKTZD AND A.ZCITEM = B.ZCITEM";
            sql += " JOIN WZ_KCDD C ON C.DWCODE=A.WERKS AND C.KCDD_CODE=A.LGORT AND C.CKH='" + DKCODE + "' ";
            sql += "   WHERE A.ZSTATUS > '02' and substr(A.ZCJRQ,1,4)= '" + year + "' and A.MATNR = '" + MATNR + "' AND substr(A.WERKS,0,3)= 'C27'";
            sql += "   GROUP BY   to_char(to_date(B.ERDAT, 'yyyy-mm-dd'), 'ww')";
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

        public DataTable GetStatusDetail(string LGPLA)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string sql = " SELECT ZSTATUS,WERKS,MATKL,MATNR,MAKTX,MEINS,SUM(GESME) AS GESME," +
                "( CASE WHEN MONTHS_BETWEEN( TO_DATE( '" +date+ " ','yyyyMMdd' ), TO_DATE( ERDAT, 'yyyyMMdd' )) > 6 THEN 01 ELSE 02 END ) AS status "+
                " FROM CONVERT_SWKC" +
                " WHERE LGPLA='" + LGPLA + "'" +
                "GROUP BY  ZSTATUS,WERKS,MATKL,MATNR,MAKTX,MEINS," +
                "( CASE WHEN MONTHS_BETWEEN( TO_DATE( '" +date+ "','yyyyMMdd' ), TO_DATE( ERDAT, 'yyyyMMdd' )) > 6 THEN 01 ELSE 02 END )"+
                " ORDER BY ZSTATUS";
            return db.GetDataTable(sql);
        }
    }
}

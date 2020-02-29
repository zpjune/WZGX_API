using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.UTILITY;

namespace UIDP.ODS.CangChu
{
    public class TotalDB
    {
        DBTool db = new DBTool("");
        /// <summary>
        /// 查询库存总资金-饼图
        /// 港东C27C  港西 C27D 油区C27G  港狮C279 港华C27B
        /// </summary>WHERE BWKEY IN('C27C','C27D','C27G''C279','C27B','C275','C274','C271')
        /// <returns></returns>
        public DataTable GetKCZJ()
        {
            string sql = @"select round(SUM(SALK3)/10000,2)  as SALK3,'TOTAL' as WERKS from CONVERT_ZWKC 
                union 
                        select round(SUM(SALK3)/10000,2)  as SALK3,'TOTALWZ' as WERKS from CONVERT_ZWKC  WHERE substr(BWKEY,1,3)='C27'
                union 
            select nvl(round(SUM(SALK3)/10000,2),0) , 'C271' from CONVERT_ZWKC WHERE BWKEY = 'C271' union 
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C274' from CONVERT_ZWKC WHERE BWKEY = 'C274' union 
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C275' from CONVERT_ZWKC WHERE BWKEY = 'C275' union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C277' from CONVERT_ZWKC WHERE BWKEY = 'C277' union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C279' from CONVERT_ZWKC WHERE BWKEY = 'C279'  union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C27B' from CONVERT_ZWKC WHERE BWKEY = 'C27B'  union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C27C' from CONVERT_ZWKC WHERE BWKEY = 'C27C'  union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C27D' from CONVERT_ZWKC WHERE BWKEY = 'C27D'  union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C27G' from CONVERT_ZWKC WHERE BWKEY = 'C27G'  union
            select nvl(round(SUM(SALK3)/10000,2),0) ,'C27I' from CONVERT_ZWKC WHERE BWKEY in('C27I','C27J','C27K','C27L')  

            ";//拼装datatable  以万为单位 四舍五入
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询实物库存-第一层按单位
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <returns></returns>
        public DataTable GetSWKCDW(string ISWZ, string WERKS,string LGPLA)
        {
            string sql = @"  select COUNT(DISTINCT MATNR) XM,WERKS,WERKS_NAME from   CONVERT_SWKC    ";
            sql += "where  KCTYPE<>3 ";
            if (ISWZ == "1")
            {
                sql += "  and substr(WERKS,1,3)='C27' AND WERKS<>'C271' ";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " and  WERKS ='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(LGPLA))
            {
                sql += "AND LGPLA LIKE '" + LGPLA + "%'";
            }
            sql += " group by WERKS,WERKS_NAME ORDER BY WERKS ";//
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询实物库存-第二层 按大类
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <returns></returns>
        public DataTable GetSWKCDL( string WERKS,string LGPLA)
        {
            string sql = @"   select   substr(MATKL,0,2)DLCODE,COUNT(DISTINCT MATNR) XM,SUM(GESME)SL,max(meins)JLDW from   CONVERT_SWKC     ";
            sql += "where  KCTYPE<>3  AND WERKS='"+WERKS+"'";
            if (!string.IsNullOrEmpty(LGPLA))
            {
                sql += "AND LGPLA LIKE '" + LGPLA + "%'";
            }
            sql += " group by substr(MATKL,0,2) order by substr(MATKL,0,2) ";//
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询实物库存-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable GetSWKC(string DLCODE,string ISWZ, string WERKS, string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string sql = @" select row_number()over(order by werks,matnr asc),sum(GESME) GESME,WERKS,WERKS_NAME,LGORT_NAME,LGORT,MAX(MATKL)MATKL,MAX(MAKTX)MAKTX,ZSTATUS,MAX(MEINS)MEINS,
                            ZSTATUS ,werks,matnr,lgort 
                            from CONVERT_SWKC  ";
            //case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            // CASE WHEN ZSTATUS='04' THEN CASE WHEN months_between(sysdate,to_date(MIN(ERDAT),'yyyy-mm-dd'))>12 then '01' else '100' end   ELSE '100' END ZT
            sql += "where  KCTYPE<>3 ";
            if (ISWZ=="1") {
                sql += "  and substr(WERKS,1,3)='C27' ";
            }
            if (!string.IsNullOrEmpty(DLCODE))
            {
                sql += " and  substr(MATKL,0,2) ='" + DLCODE + "'";
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += " and  WERKS ='" + WERKS + "'";
            }
            if (!string.IsNullOrEmpty(WERKS_NAME))
            {
                sql += " and  WERKS_NAME like'%" + WERKS_NAME + "%'";
            }
            if (!string.IsNullOrEmpty(LGORTNAME))
            {
                sql += " and  LGORT_NAME like'%" + LGORTNAME + "%'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  MATKL like'%" + MATKL + "%'";
            }
            sql += "group by werks,matnr,lgort,zstatus,WERKS_NAME,LGORT_NAME  ";//
            return db.GetDataTable(sql);
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
        public DataTable GetTotalJYWZ(string ISWZ, string WERKS)
        {
            string sql = " SELECT a.WERKS,a.WERKS_NAME,COUNT(DISTINCT a.MATNR) AS SL,SUM( a.GESME*NVL(b.DANJIA,0) ) AS SALK3 FROM CONVERT_SWKC a" +
                " LEFT JOIN CONVERT_ZWKC b ON a.WERKS=b.BWKEY AND a.MATNR=b.MATNR" +
                " {0}" +
                " where months_between(sysdate,to_date(a.ERDAT,'yyyy-mm-dd'))>12" +
                " AND a.KCTYPE=0" +
                " AND c.CKH IS NOT NULL" +
                " {1}";
            if (ISWZ == "1")
            {
                sql = string.Format(sql, "LEFT JOIN WZ_KCDD c ON a.WERKS=c.DWCODE AND a.LGORT=c.KCDD_CODE", " AND c.CKH IS NOT NULL and substr(a.WERKS,1,3)='C27'");
            }
            else
            {
                sql = string.Format(sql, "", "");
            }
            if (!string.IsNullOrEmpty(WERKS))
            {
                sql += "  and a.WERKS='" + WERKS + "'";
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
        public DataTable GetDLJYWZ(string ISWZ, string WERKS)
        {
            string sql = " select SUBSTR(a.MATKL, 0,2)AS DLCODE,COUNT(DISTINCT a.MATNR) AS SL,SUM(a.GESME) AS GESME,a.MEINS,SUM( a.GESME*NVL(b.DANJIA,0) )AS SALK3" +
                 " FROM CONVERT_SWKC a" +
                 " LEFT JOIN CONVERT_ZWKC b ON a.WERKS=b.BWKEY AND a.MATNR=b.MATNR" +
                 " {0}" +
                 " where a.WERKS='" + WERKS + "'" +
                 " AND months_between(SYSDATE,to_date( a.ERDAT, 'yyyy-mm-dd' )) > 12 " +
                 " AND a.KCTYPE=0" +
                 " {1}";
            if (ISWZ == "1")
            {
                sql = string.Format(sql, "LEFT JOIN WZ_KCDD c ON a.WERKS=c.DWCODE AND a.LGORT=c.KCDD_CODE", " AND c.CKH IS NOT NULL and substr(a.WERKS,1,3)='C27'");
            }
            else
            {
                sql = string.Format(sql, "", "");
            }
            sql += " GROUP BY SUBSTR(a.MATKL, 0,2),a.MEINS" +
                " ORDER BY SUBSTR(a.MATKL, 0,2)";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询积压物资-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable GetJYWZ(string DLCODE,string ISWZ,string MEINS,string WERKS, string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string sql = @" select row_number()over(order by a.werks,a.matnr asc),sum(a.GESME) GESME,a.WERKS,a.WERKS_NAME,a.LGORT_NAME,a.LGORT,MAX(a.MATKL)MATKL,MAX(a.MAKTX)MAKTX,a.ZSTATUS,MAX(a.MEINS)MEINS,
                            '积压' ZT
                               ,a.werks,a.matnr,a.lgort,a.GESME*NVL(b.DANJIA,0)AS SALK3
                            from CONVERT_SWKC a ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += " LEFT JOIN CONVERT_ZWKC b ON a.WERKS = b.BWKEY AND a.MATNR = b.MATNR" +
                " {0}";
            sql += " where months_between(sysdate,to_date(a.ERDAT,'yyyy-mm-dd'))>12 " +
                " AND SUBSTR(a.MATKL,0,2)='" + DLCODE + "'" +
                " AND a.MEINS='"+MEINS+"'" +
                " {1}";
            if (ISWZ == "1")
            {
                sql = string.Format(sql, "LEFT JOIN WZ_KCDD c ON a.WERKS=c.DWCODE AND a.LGORT=c.KCDD_CODE", " AND c.CKH IS NOT NULL and substr(a.WERKS,1,3)='C27'");
            }
            else
            {
                sql = string.Format(sql, "", "");
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
        /// 总库存-出库入库统计-按年 按月统计金额
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet GetCRKJE(string year,int ISWZ)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();//以万为单位
            string sql = @"select sum(JE)/10000 JE,substr(BUDAT_MKPF,5,2) Month
                            from CONVERT_CKJE
                            WHERE  substr(BUDAT_MKPF,1,4)='"+year+"'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(WERKS, 0, 3)='C27'";
            }         
            sql +="  GROUP BY substr(BUDAT_MKPF,5,2) ORDER BY substr(BUDAT_MKPF,5,2)";
            d.Add("CKJE", sql);
            sql = @"select sum(JE)/10000 JE,substr(BUDAT_MKPF,5,2) Month
                    from CONVERT_RKJE
                    WHERE  substr(BUDAT_MKPF,1,4)='"+year+"'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(WERKS, 0, 3)='C27'";
            }
            sql +="  GROUP BY substr(BUDAT_MKPF,5,2) ORDER BY substr(BUDAT_MKPF,5,2)";
            d.Add("RKJE", sql);
            return db.GetDataSet(d);
        }
        public DataSet getCRKDetail(string year, string month, int ISWZ)
        {
            #region MyRegion
            /*
             string sql = @" select DISTINCT A.WERKS_NAME
                            from CONVERT_CKJE A 
                            join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                            LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            sql += " UNION ";
            sql += @"  select DISTINCT B.CKH_NAME
                            from CONVERT_RKJE A
                            join WZ_KCDD B on A.WERKS = B.DWCODE AND A.LGORT = B.KCDD_CODE
                            LEFT JOIN WZ_CRKL C ON DK_CODE = B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            d.Add("DK_NAME", sql);//大库名称
            sql = @"select MAX(B.CKH_NAME) CKH_NAME,sum(JE)/10000 CKJE,SUM(C.CKL) CKL
                    from CONVERT_CKJE A
                    join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                    LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            sql += "   GROUP BY  B.CKH";
            d.Add("CKJE_Detail", sql);//出库金额明细
            sql = @"select MAX(B.CKH_NAME) CKH_NAME,sum(JE)/10000 RKJE,SUM(C.RKL) RKL
                    from CONVERT_RKJE A
                    join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                    LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            sql += "   GROUP BY  B.CKH";
            d.Add("RKJE_Detail", sql);//入库金额明细
             */
            #endregion

            Dictionary<string, string> d = new Dictionary<string, string>();//以万为单位
            string sql = @" select DISTINCT A.WERKS_NAME
                            from CONVERT_CKJE A  ";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            sql += " UNION ";
            sql += @"  select DISTINCT A.WERKS_NAME
                            from CONVERT_RKJE A
                             ";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            d.Add("DK_NAME", sql);//大库名称
            sql = @"select A.WERKS_NAME ,sum(JE)/10000 CKJE
                    from CONVERT_CKJE A
                     ";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            sql += "   GROUP BY A.WERKS_NAME ";
            d.Add("CKJE_Detail", sql);//出库金额明细
            sql = @"select A.WERKS_NAME,sum(JE)/10000 RKJE
                    from CONVERT_RKJE A ";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            if (ISWZ == 2)
            {
                sql += " AND SUBSTR(A.WERKS, 0, 3)='C27'";
            }
            sql += "   GROUP BY  A.WERKS_NAME ";
            d.Add("RKJE_Detail", sql);//入库金额明细
            return db.GetDataSet(d);
        }
        /// <summary>
        /// 总库查询-保管员工作量
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable getBGYGZL(string month, string workerName)
        {
            //不带紧急出入库的保管员工作量查询
            string sql = @"select DISTINCT ERNAME,a.WORKER_NAME,COUNT(*)XMHJ,c.NAME AS WERKS_NAME,max(substr(ERDAT,1,6))NIANYUE,
                            SUM(CASE WHEN JBJLDW='吨' then NSOLM ELSE 0 END ) HJ_DUN,
                            SUM(CASE WHEN JBJLDW='米' then NSOLM ELSE 0 END ) HJ_MI,
                            SUM(CASE WHEN JBJLDW<>'米' AND JBJLDW<>'吨' then NSOLM ELSE 0 END ) HJ_QT,
                            sum(CASE WHEN substr(TZD,1,1)='1' then 1 ELSE 0 END ) RKHJ,
                            SUM(CASE WHEN JBJLDW='吨' and substr(TZD,1,1)='1' then NSOLM ELSE 0 END ) RK_DUN,
                            SUM(CASE WHEN JBJLDW='米'  and substr(TZD,1,1)='1' then NSOLM ELSE 0 END ) RK_MI,
                            SUM(CASE WHEN JBJLDW<>'米' AND JBJLDW<>'吨' and substr(TZD,1,1)='1' then NSOLM ELSE 0 END ) RK_QT,
                            sum(CASE WHEN substr(TZD,1,1)='2' then 1 ELSE 0 END ) CKHJ,
                            SUM(CASE WHEN JBJLDW='吨' and substr(TZD,1,1)='2' then NSOLM ELSE 0 END ) CK_DUN,
                            SUM(CASE WHEN JBJLDW='米'  and substr(TZD,1,1)='2' then NSOLM ELSE 0 END )  CK_MI,
                            SUM(CASE WHEN JBJLDW<>'米' AND JBJLDW<>'吨' and substr(TZD,1,1)='2' then NSOLM ELSE 0 END )  CK_QT
                            from
                            CONVERT_BGYGZL a 
                            JOIN WZ_BGY b ON a.ERNAME=b.WORKER_CODE
                            LEFT JOIN TS_DICTIONARY c ON b.CKH=c.CODE AND c.PARENTCODE='TOTAL'";

            sql += "   where substr(ERDAT,1,6)=substr('" + month + "',1,6)";
            if (!string.IsNullOrEmpty(workerName))
            {
                sql += " and  WORKER_NAME like '%" + workerName + "%'";
            }
            sql += "  group by ERNAME,a.WORKER_NAME,c.NAME";
            return db.GetDataTable(sql);
            #region 保管员工作量，带紧急出入库信息的 20200229 YZ
            /***
             *以下查询逻辑如下 
             * 1.正常查询 CONVERT_BGYGZL表内的保管员工作量
             * 2.查询紧急入库表内的保管员工作量，入库字段正常查询，出库字段全部返回0
             * 3.查询紧急出库表内的保管员工作量，出库字段正常查询，入库字段全部返回0
             * 4.将上述3个表通过UNION ALL 拼合在一起
             * 5.通过查询拼合三个表的数据对总数进行查询
             ***/
            ///总查询字段
            //string PartSql = "SELECT t.ERNAME,t.WORKER_NAME,COUNT(*)AS XMHJ,t.WERKS_NAME,t.NIANYUE," +
            //    " SUM(t.RK_DUN+t.CK_DUN) AS HJ_DUN," +
            //    " SUM(t.RK_MI+t.CK_MI) AS HJ_MI," +
            //    " SUM(t.RK_QT+t.CK_QT) AS HJ_QT," +
            //    " SUM(t.RKHJ) AS RKHJ," +
            //    " SUM(t.RK_DUN) AS RK_DUN," +
            //    " SUM(t.RK_MI) AS RK_MI," +
            //    " SUM(t.RK_QT) AS RK_QT," +
            //    " SUM(t.CKHJ) AS CKHJ," +
            //    " SUM(t.CK_DUN) AS CK_DUN," +
            //    " SUM(t.CK_MI) AS CK_MI," +
            //    " SUM(t.CK_QT) AS CK_QT" +
            //    " FROM({0} UNION ALL {1} UNION ALL {2})t" +
            //      " GROUP BY t.ERNAME,t.WORKER_NAME,t.WERKS_NAME,t.NIANYUE";
            ///***
            // * 下面代码是查询保管员工作量的sql
            // ***/
            //string BGYSql = " SELECT " +
            //    " CAST(ERNAME AS NVARCHAR2(100)) AS ERNAME," +
            //    " CAST(a.WORKER_NAME AS NVARCHAR2(100)) AS WORKER_NAME ," +
            //    " CAST(c.NAME AS NVARCHAR2(100)) AS WERKS_NAME," +
            //    " CAST(substr( ERDAT, 1, 6 ) AS NVARCHAR2(100)) AS NIANYUE," +
            //    " CAST(( CASE WHEN substr( TZD, 1, 1 ) = '1' THEN 1 ELSE 0 END ) AS DECIMAL(18,2)) AS RKHJ," +
            //    " CAST(( CASE WHEN JBJLDW = '吨' AND substr( TZD, 1, 1 ) = '1' THEN NSOLM ELSE 0 END )AS DECIMAL(18,2)) AS RK_DUN," +
            //    " CAST(( CASE WHEN JBJLDW = '米' AND substr( TZD, 1, 1 ) = '1' THEN NSOLM ELSE 0 END )AS DECIMAL(18,2)) AS RK_MI," +
            //    " CAST(( CASE WHEN JBJLDW <> '米' AND JBJLDW <> '吨' AND substr( TZD, 1, 1 ) = '1' THEN NSOLM ELSE 0 END )AS DECIMAL(18,2)) AS RK_QT," +
            //    " CAST((CASE WHEN substr(TZD,1,1)='2' then 1 ELSE 0 END) AS DECIMAL(18,2)) AS CKHJ," +
            //    " CAST(( CASE WHEN JBJLDW = '吨' AND substr( TZD, 1, 1 ) = '2' THEN NSOLM ELSE 0 END )AS DECIMAL(18,2))AS  CK_DUN," +
            //    " CAST(( CASE WHEN JBJLDW = '米' AND substr( TZD, 1, 1 ) = '2' THEN NSOLM ELSE 0 END )AS DECIMAL(18,2)) AS CK_MI," +
            //    " CAST(( CASE WHEN JBJLDW <> '米' AND JBJLDW <> '吨' AND substr( TZD, 1, 1 ) = '2' THEN NSOLM ELSE 0 END ) AS DECIMAL(18,2))AS CK_QT " +
            //    " FROM CONVERT_BGYGZL a " +
            //    " JOIN WZ_BGY b ON a.ERNAME = b.WORKER_CODE" +
            //    " LEFT JOIN TS_DICTIONARY c ON b.CKH = c.CODE" +
            //    " AND c.PARENTCODE = 'TOTAL' " +
            //    " WHERE" +
            //    " substr( ERDAT, 1, 6 ) = substr( '"+ month+"', 1, 6 ) " +
            //    " {0}";
            ///***
            // * 紧急出入库查询表字段基本一致，支
            // ***/
            //string JJRKSql = " SELECT " +
            //    " CAST(b.USER_CODE AS NVARCHAR2(100)) AS ERNAME," +
            //    " CAST(b.USER_NAME AS NVARCHAR2(100)) AS WORKER_NAME," +
            //    " CAST(d.NAME AS NVARCHAR2(100))AS WERKS_NAME," +
            //    " CAST(TO_CHAR(BGY_DATE,'yyyyMM') AS NVARCHAR2(100)) AS NIANYUE," +
            //    " CAST( 1 AS DECIMAL(18,2)) AS RKHJ," +
            //    " CAST(( CASE WHEN a.MEINS = '吨'  THEN TOTALPRICE1 ELSE 0 END )AS DECIMAL(18,2)) AS RK_DUN," +
            //    " CAST(( CASE WHEN a.MEINS = '米'  THEN TOTALPRICE1 ELSE 0 END )AS DECIMAL(18,2)) AS RK_MI," +
            //    " CAST(( CASE WHEN a.MEINS <> '米' AND a.MEINS <> '吨'  THEN TOTALPRICE1 ELSE 0 END )AS DECIMAL(18,2)) AS RK_QT," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS CKHJ," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS CK_DUN," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS CK_MI," +
            //    " CAST(0 AS DECIMAL(18,2)) AS CK_QT" +
            //    " FROM JJRK a" +
            //    " JOIN TS_UIDP_USERINFO b ON a.BGY_ID=b.USER_ID" +
            //    " JOIN WZ_KCDD c ON EXISTS (SELECT 1 FROM TS_UIDP_ORG WHERE ORG_CODE = a.DW_CODE AND c.DWCODE=DW_CODE)  AND a.KCDD=c.KCDD_CODE" +
            //    " JOIN TS_DICTIONARY d ON c.CKH=d.CODE AND d.PARENTCODE='TOTAL'" +
            //    " WHERE TO_CHAR(BGY_DATE,'yyyyMM')='" + month + "'" +
            //    " AND a.APPROVAL_STATUS>=5 " +
            //    " {0}";
            //string JJCKSql = " SELECT " +
            //    " CAST(b.USER_CODE AS NVARCHAR2(100)) AS ERNAME," +
            //    " CAST(b.USER_NAME AS NVARCHAR2(100)) AS WORKER_NAME," +
            //    " CAST(d.NAME AS NVARCHAR2(100))AS WERKS_NAME," +
            //    " CAST(TO_CHAR(BGY_DATE,'yyyyMM') AS NVARCHAR2(100)) AS NIANYUE," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS RKHJ," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS RK_DUN," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS RK_MI," +
            //    " CAST( 0 AS DECIMAL(18,2)) AS RK_QT," +
            //    " CAST( 1 AS DECIMAL(18,2)) AS CKHJ," +
            //    " CAST(( CASE WHEN a.MEINS = '吨'  THEN TOTALPRICE1 ELSE 0 END )AS DECIMAL(18,2)) AS CK_DUN," +
            //    " CAST(( CASE WHEN a.MEINS = '米'  THEN TOTALPRICE1 ELSE 0 END )AS DECIMAL(18,2)) AS CK_MI," +
            //    " CAST(( CASE WHEN a.MEINS <> '米' AND a.MEINS <> '吨'  THEN TOTALPRICE1 ELSE 0 END )AS DECIMAL(18,2)) AS CK_QT" +
            //    " FROM JJCK a" +
            //    " JOIN TS_UIDP_USERINFO b ON a.BGY_ID=b.USER_ID" +
            //    " JOIN WZ_KCDD c ON EXISTS (SELECT 1 FROM TS_UIDP_ORG WHERE ORG_CODE = a.DW_CODE AND c.DWCODE=DW_CODE)  AND a.KCDD=c.KCDD_CODE" +
            //    " JOIN TS_DICTIONARY d ON c.CKH=d.CODE AND d.PARENTCODE='TOTAL'" +
            //    " WHERE TO_CHAR(BGY_DATE,'yyyyMM')='"+ month+"' " +
            //    " AND a.APPROVAL_STATUS>=5" +
            //    " {0}";
            //if (!string.IsNullOrEmpty(workerName))
            //{
            //    BGYSql = string.Format(BGYSql," and  b.WORKER_NAME like '%" + workerName + "%'");
            //    JJRKSql = string.Format(JJRKSql, " and b.USER_NAME like '%" + workerName + "%'");
            //    JJCKSql = string.Format(JJCKSql," and b.USER_NAME like '%" + workerName + "%'");
            //}
            //else
            //{
            //    BGYSql = string.Format(BGYSql, "");
            //    JJRKSql = string.Format(JJRKSql, "");
            //    JJCKSql = string.Format(JJCKSql, "");
            //}
            //string TotalSql = string.Format(PartSql, BGYSql, JJRKSql, JJCKSql);            
            //return db.GetDataTable(TotalSql);
            #endregion
        }
        /// <summary>
        /// 总库存保管员工作量明细查询
        /// </summary>
        /// <param name="nianyue">年月</param>
        /// <param name="TZDType">1 入库单 2 出库单</param>
        /// <param name="workerCode">员工编号</param>
        /// <returns></returns>
        public DataTable getBGYGZLDetail(string nianyue, string TZDType, string workerCode)
        {
            string sql = "select * from CONVERT_BGYGZL ";
            sql += " where substr(ERDAT,1,6)=substr('" + nianyue + "',1,6) ";
            sql += " and ERNAME='" + workerCode + "' AND substr(TZD,1,1)='" + TZDType + "' ";
            sql += " ORDER BY TZD,ITEMS ";
            #region 以下是保管员工作量详情，带紧急出入库信息的 20200229 YZ
            //string UnionTableName = string.Empty;
            //if (TZDType == "1")
            //{
            //    UnionTableName = "JJRK";
            //}
            //else
            //{
            //    UnionTableName = "JJCK";
            //}
            //string sql = "SELECT " +
            //    " CAST(WERKS as NVARCHAR2(100)) AS WERKS," +
            //    " CAST(WERKS_NAME as NVARCHAR2(100)) AS WERKS_NAME," +
            //    " CAST(TZD as NVARCHAR2(100)) AS TZD," +
            //    " CAST(ITEMS as DECIMAL(18,2)) AS WERKS," +
            //    " CAST(MAKTX as NVARCHAR2(100)) AS MAKTX," +
            //    " CAST(JBJLDW as NVARCHAR2(100)) AS JBJLDW," +
            //    " CAST(NSOLM as DECIMAL(18,2)) AS NSOLM," +
            //    " CAST(ERDAT as NVARCHAR2(100)) AS ERDAT" +
            //    " FROM CONVERT_BGYGZL" +
            //    " WHERE substr( TZD, 1, 1 ) ='" + TZDType + "'" +
            //    " AND SUBSTR(WERKS,0,3)='C27'" +
            //    " {0} {1}" +//两个where条件 下面的union拼出来的sql也要用到
            //    " UNION ALL" +//上面为保管员工作量模型表，下面为紧急出入库表
            //    " SELECT" +
            //    " CAST(c.DW_CODE as NVARCHAR2(100)) AS WERKS," +
            //    " CAST(c.ORG_NAME as NVARCHAR2(100)) AS WERKS_NAME," +
            //    " CAST(a.CODE  as NVARCHAR2(100)) AS TZD," +
            //    " CAST('' as DECIMAL(18,2)) AS WERKS," +
            //    " CAST(a.MATNX as NVARCHAR2(100)) AS MAKTX," +
            //    " CAST(a.MEINS as NVARCHAR2(100)) AS JBJLDW," +
            //    " CAST(a.RKNUMBER1 as DECIMAL(18,2)) AS NSOLM," +
            //    " CAST(TO_CHAR( BGY_DATE, 'yyyyMMdd' ) as NVARCHAR2(100)) AS ERDAT" +
            //    " FROM {2} a" +//紧急出入库表名
            //    " JOIN TS_UIDP_USERINFO b ON a.BGY_ID = b.USER_ID" +
            //    " JOIN TS_UIDP_ORG c ON a.DW_CODE = c.ORG_CODE" +
            //    " WHERE a.APPROVAL_STATUS >= 5" +//保管员提交或者撤销订单状态
            //    " AND SUBSTR(c.DW_CODE,0,3)='C27'" +
            //    " {3} {4}";
            //sql = string.Format(sql, "AND substr( ERDAT, 1, 6 ) = substr( '"+nianyue+"',0,6)", " AND ERNAME='"+workerCode+"'", UnionTableName, " AND b.USER_CODE='"+workerCode+"'", " AND TO_CHAR( BGY_DATE, 'yyyyMM' ) = SUBSTR('"+nianyue+"',0,6)");
            #endregion
            return db.GetDataTable(sql);

        }
        ///重点物资储备-左侧菜单
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCB(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string sql = @" select sum(A.GESME) GESME,A.WERKS,A.WERKS_NAME,A.LGORT_NAME,A.LGORT,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,A.ZSTATUS,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        left join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS
                        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where  A.KCTYPE<>3  ";
            if (!string.IsNullOrEmpty(WERKS_NAME))
            {
                sql += " and  A.WERKS_NAME like'%" + WERKS_NAME + "%'";
            }
            if (!string.IsNullOrEmpty(LGORTNAME))
            {
                sql += " and  A.LGORT_NAME like'%" + LGORTNAME + "%'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            sql += "  group by A.WERKS,A.MATNR,A.LGORT,A.WERKS_NAME,A.LGORT_NAME ,A.ZSTATUS ";//
            return db.GetDataTable(sql);
        }
        ///重点物资储备-总库存
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCBTOTAL( string MATNR, string MATKL, string MAKTX)
        {
            string sql = @" select sum(A.GESME) GESME,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS";// zstatus 是表示上架还是质检（未上架）状态
            sql += " where  A.KCTYPE<>3 AND substr(werks,0,3)='C27' " +
                " and C.CKH IS NOT NULL";       
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
                    " or a.MAKTX LIKE '%润滑剂%') ";
            }
            else
            {
                sql += " AND a.MAKTX LIKE '%" + MAKTX + "%'";
            }
            sql += "  group by A.MATNR,B.WL_SORT ORDER BY B.WL_SORT";//
            return db.GetDataTable(sql);
        }
        ///重点物资储备明细-总库存
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataTable getZDWZCBTOTALDETAIL(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string sql = @" select sum(A.GESME) GESME,A.WERKS,A.WERKS_NAME,A.LGORT_NAME,A.LGORT,
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,A.ZSTATUS,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        left join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS
                        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                        ";// zstatus 是表示上架还是质检（未上架）状态
            sql += "where  A.KCTYPE<>3  AND substr(werks,0,3)='C27'" +
                " AND C.CKH IS NOT NULL";
            if (!string.IsNullOrEmpty(WERKS_NAME))
            {
                sql += " and  A.WERKS_NAME like'%" + WERKS_NAME + "%'";
            }
            if (!string.IsNullOrEmpty(LGORTNAME))
            {
                sql += " and  A.LGORT_NAME like'%" + LGORTNAME + "%'";
            }
            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " and  A.MATNR like'%" + MATNR + "%'";
            }
            if (!string.IsNullOrEmpty(MATKL))
            {
                sql += " and  A.MATKL like'%" + MATKL + "%'";
            }
            sql += "  group by A.WERKS,A.MATNR,A.LGORT,A.WERKS_NAME,A.LGORT_NAME ,A.ZSTATUS  ";//
            return db.GetDataTable(sql);
        }
        ///重点物资出入库查询-总库存
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        public DataSet getZDWZCRK(string year, string MATNR)
        {
            Dictionary<string, string> dc = new Dictionary<string, string>();
            string sql = "select SUM(MAXHAVING)MAXHAVING,SUM(MINHAVING)MINHAVING";
            sql += " FROM WZ_ZDWZWH WHERE WL_CODE = '"+MATNR+"'";
            sql += "  GROUP BY WL_CODE ";
            dc.Add("zgcb",sql);
            sql = "  select SUM(GESME)GESME";
            sql += " FROM CONVERT_SWKC_RECORD WHERE MATNR = '"+ MATNR + "'  and KCTYPE<>3 AND substr(WERKS,0,3)= 'C27' and substr(DLDATE,0,4)='" + year+"'";
            sql += "  GROUP BY DLDATE ";
            dc.Add("kc",sql);
            sql = "select sum(ZSJDHSL)ZSJDHSL,to_char(to_date(B.ERDAT,'yyyy-mm-dd'),'ww')WEEK";
            sql += " from ZC10MMDG072 A";
            sql += "     join ZC10MMDG085A B on A.ZDHTZD = B.ZDHTZD AND A.ZITEM = B.ZITEM";
            sql += " WHERE A.ZSTATUS > '03' and substr(A.ZCJRQ,1,4)= '"+ year + "' and A.MATNR = '"+MATNR+"' AND substr(A.WERKS,0,3)= 'C27'";
            sql += " GROUP BY   to_char(to_date(B.ERDAT, 'yyyy-mm-dd'), 'ww')";
            dc.Add("rk", sql);
            sql = " select sum(ZSJFHSL)ZSJFHSL,to_char(to_date(B.ERDAT,'yyyy-mm-dd'),'ww')WEEK";
            sql += "   from ZC10MMDG078 A";
            sql += "   join ZC10MMDG085A B on A.ZCKTZD = B.ZCKTZD AND A.ZCITEM = B.ZCITEM";
            sql += "   WHERE A.ZSTATUS > '02' and substr(A.ZCJRQ,1,4)= '"+year+"' and A.MATNR = '"+MATNR+"' AND substr(A.WERKS,0,3)= 'C27'";
            sql += "   GROUP BY   to_char(to_date(B.ERDAT, 'yyyy-mm-dd'), 'ww')";
            dc.Add("ck", sql);
            return db.GetDataSet(dc);

            //string sql = @" 
            //            select G.*,H.RKSL,I.RKSUMSL,J.CKSL,K.CKSUMSL,F.WL_CODE,'"+yearmonth+"' MONTH ";
            //  sql += @"           from WZ_ZDWZPZ F
            //             LEFT JOIN (
            //            select sum(A.GESME) GESME，
            //            MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
            //            from CONVERT_SWKC A
            //            JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR AND A.KCTYPE<>3 
            //            left join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS
            //            left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
            //             group by A.MATNR) G ON F.WL_CODE=G.MATNR
            //            LEFT JOIN 
            //            (
            //            select SUM(A.ZDHSL) RKSL,A.MATNR
            //            from ZC10MMDG072 A 
            //            JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //sql += "     WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            //sql += @"         GROUP BY A.MATNR) H ON F.WL_CODE=H.MATNR

            //            LEFT JOIN 
            //            (
            //            select SUM(A.ZDHSL) RKSUMSL ,A.MATNR
            //            from ZC10MMDG072 A 
            //           JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //sql += "        WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            //sql += @"       GROUP BY A.MATNR) I ON F.WL_CODE=I.MATNR
            //            LEFT JOIN 
            //            (
            //            select SUM(A.ZFHSL) CKSL,A.MATNR
            //            from ZC10MMDG078 A 
            //            JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //sql += "             WHERE A.ZSTATUS>'03' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            //sql += @"         GROUP BY A.MATNR) J ON F.WL_CODE=J.MATNR

            //            LEFT JOIN 
            //            (
            //            select SUM(A.ZFHSL) CKSUMSL ,A.MATNR
            //            from ZC10MMDG078 A 
            //            JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            //sql += "          WHERE A.ZSTATUS>'03' and substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            //sql += "         GROUP BY A.MATNR) K ON F.WL_CODE=K.MATNR ";
            
            //    sql += " where  F.MATNR='" + MATNR + "'";
        }
        /// <summary>
        /// 重点物资出入库明细-去向明细
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MONTH"></param>
        /// <returns></returns>
        public DataTable getZDWZCRKDetail(string MATNR, string MONTH) {
            string sql = @" select MAX(CASE WHEN  C.DW_NAME IS NULL THEN B.WEMPF ELSE  C.DW_NAME  END)  WERKS_NAME,SUM(A.ZFHSL) SL
                        from ZC10MMDG078 A
                        JOIN MSEG B ON A.MBLNR=B.MBLNR AND A.ZEILE=B.ZEILE
                        LEFT JOIN WZ_DW C ON C.DW_CODE=B.WEMPF";
                   sql+="     WHERE  A.MATNR='"+MATNR+"'   and  substr(A.ZCJRQ,1,4)=substr('"+MONTH+ "',1,4) and substr(A.ZCJRQ,5,2)<=substr('" + MONTH + "',5,2)   group by B.WEMPF";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询重点物资配置
        /// </summary>
        /// <param name="WL_NAME"></param>
        /// <returns></returns>
        public DataTable getZDWZPZ(string WL_NAME)
        {
            string SQL = "select WL_CODE,WL_NAME from WZ_ZDWZPZ ";
            if (!string.IsNullOrWhiteSpace(WL_NAME))
            {
                if (WL_NAME== "钻井泥浆材料") {
                    SQL += " WHERE WL_NAME LIKE '%封闭剂%' or WL_NAME LIKE '%膨润土粉%' or WL_NAME LIKE '%片碱%' or WL_NAME LIKE '%纯碱%'  or WL_NAME LIKE '%堵漏剂%' or WL_NAME LIKE '%润滑剂%'  ";
                }
                else {
                    SQL += " WHERE WL_NAME LIKE '%" + WL_NAME + "%'";
                }
                
            }
            SQL += " ORDER BY  WL_CODE ";
            return db.GetDataTable(SQL);
        }
    }
}

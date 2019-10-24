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
        /// </summary>
        /// <returns></returns>
        public DataTable GetKCZJ()
        {
            string sql = @"select round(SUM(SALK3)/10000,2)  as SALK3,'TOTAL' as WERKS from CONVERT_ZWKC WHERE BWKEY IN('C27B','C27C','C27D','C27G''C279','C27B')
                union 
            select round(SUM(SALK3)/10000,2) , 'C27C' from CONVERT_ZWKC WHERE BWKEY = 'C27C' union 
            select round(SUM(SALK3)/10000,2) ,'C27D' from CONVERT_ZWKC WHERE BWKEY = 'C27D' union 
            select round(SUM(SALK3)/10000,2) ,'C27G' from CONVERT_ZWKC WHERE BWKEY = 'C27G' union 
            select round(SUM(SALK3)/10000,2) ,'C279' from CONVERT_ZWKC WHERE BWKEY = 'C279' union 
            select round(SUM(SALK3)/10000,2) ,'C27B' from CONVERT_ZWKC WHERE BWKEY = 'C27B' ";//拼装datatable  以万为单位 四舍五入
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
        public DataTable GetSWKC(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string sql = @" select sum(GESME) GESME,WERKS,WERKS_NAME,LGORT_NAME,LGORT,MAX(MATKL)MATKL,MAX(MAKTX)MAKTX,ZSTATUS,MAX(MEINS)MEINS,
                            CASE WHEN ZSTATUS='04' THEN CASE WHEN months_between(sysdate,to_date(MIN(ERDAT),'yyyy-mm-dd'))>6 then '01' else '100' end   ELSE '100' END ZT
                               ,werks,matnr,lgort 
                            from CONVERT_SWKC  ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += "where 1=1 ";
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
            sql += "group by werks,matnr,lgort,zstatus,WERKS_NAME,LGORT_NAME ";//
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
        public DataTable GetJYWZ(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string sql = @" select sum(GESME) GESME,WERKS,WERKS_NAME,LGORT_NAME,LGORT,MAX(MATKL)MATKL,MAX(MAKTX)MAKTX,ZSTATUS,MAX(MEINS)MEINS,
                            '积压' ZT
                               ,werks,matnr,lgort 
                            from CONVERT_SWKC  ";//case when 用来判断状态zt是否过期 积压等状态  01 积压 02报废活超期 03 有保存期限  其他为正常（100）， zstatus 是表示上架还是质检（未上架）状态
            sql += "where months_between(sysdate,to_date(ERDAT,'yyyy-mm-dd'))>6";
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
            sql += "group by werks,matnr,lgort,zstatus,WERKS_NAME,LGORT_NAME ";//
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 总库存-出库入库统计-按年 按月统计金额
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet GetCRKJE(string year)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();//以万为单位
            string sql = @"select sum(JE)/10000 JE,substr(BUDAT_MKPF,5,2) Month
                            from CONVERT_CKJE
                            WHERE  substr(BUDAT_MKPF,1,4)='";
            sql += year + "'  GROUP BY substr(BUDAT_MKPF,5,2) ORDER BY substr(BUDAT_MKPF,5,2)";
            d.Add("CKJE", sql);
            sql = @"select sum(JE)/10000 JE,substr(BUDAT_MKPF,5,2) Month
                    from CONVERT_RKJE
                    WHERE  substr(BUDAT_MKPF,1,4)='";
            sql += year + "'  GROUP BY substr(BUDAT_MKPF,5,2) ORDER BY substr(BUDAT_MKPF,5,2)";
            d.Add("RKJE", sql);
            return db.GetDataSet(d);
        }
        public DataSet getCRKDetail(string year, string month)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();//以万为单位
            string sql = @" select DISTINCT B.CKH_NAME
                            from CONVERT_CKJE A 
                            join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                            LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            sql += " UNION ";
            sql += @"  select DISTINCT B.CKH_NAME
                            from CONVERT_RKJE A
                            join WZ_KCDD B on A.WERKS = B.DWCODE AND A.LGORT = B.KCDD_CODE
                            LEFT JOIN WZ_CRKL C ON DK_CODE = B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";

            d.Add("DK_NAME", sql);//大库名称
            sql = @"select MAX(B.CKH_NAME) CKH_NAME,sum(JE)/10000 CKJE,SUM(C.CKL) CKL
                    from CONVERT_CKJE A
                    join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                    LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            sql += "   GROUP BY  B.CKH";
            d.Add("CKJE_Detail", sql);//出库金额明细
            sql = @"select MAX(B.CKH_NAME) CKH_NAME,sum(JE)/10000 RKJE,SUM(C.RKL) RKL
                    from CONVERT_RKJE A
                    join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                    LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            sql += "   GROUP BY  B.CKH";
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
            string sql = @"select ERNAME,WORKER_NAME,WERKS_NAME,COUNT(*)XMHJ,max(substr(ERDAT,1,6))NIANYUE,
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
                            CONVERT_BGYGZL ";

            sql += "   where substr(ERDAT,1,6)=substr('" + month + "',1,6)";
            if (!string.IsNullOrEmpty(workerName))
            {
                sql += " and  WORKER_NAME like '%" + workerName + "%'";
            }
            sql += "  group by ERNAME,WORKER_NAME,WERKS_NAME";
            return db.GetDataTable(sql);
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
            return db.GetDataTable(sql);

        }
        ///重点物资储备-总库存
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
            sql += "where 1=1 ";
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
        public DataTable getZDWZCRK(string yearmonth, string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL)
        {
            string year = yearmonth.Substring(0, 4);
            string _month = yearmonth.Substring(4, 2);
            string sql = @" 
                        select G.*,H.RKSL,I.RKSUMSL,J.CKSL,K.CKSUMSL,F.WL_CODE,'"+yearmonth+"' MONTH ";
              sql += @"           from WZ_ZDWZPZ F
                         LEFT JOIN (
                        select sum(A.GESME) GESME，
                        MAX(A.MATKL)MATKL,MAX(A.MAKTX)MAKTX,MAX(A.MEINS)MEINS, A.MATNR,MAX(D.MAXHAVING)MAXHAVING,MAX(MINHAVING)MINHAVING
                        from CONVERT_SWKC A
                        JOIN WZ_ZDWZPZ B ON B.WL_CODE=A.MATNR
                        left join WZ_KCDD C ON C.KCDD_CODE=A.LGORT AND C.DWCODE=A.WERKS
                        left join WZ_ZDWZWH D ON D.KC_CODE=C.CKH AND D.WL_CODE=A.MATNR
                         group by A.MATNR) G ON F.WL_CODE=G.MATNR
                        LEFT JOIN 
                        (
                        select SUM(A.ZDHSL) RKSL,A.MATNR
                        from ZC10MMDG072 A 
                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "     WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            sql += @"         GROUP BY A.MATNR) H ON F.WL_CODE=H.MATNR

                        LEFT JOIN 
                        (
                        select SUM(A.ZDHSL) RKSUMSL ,A.MATNR
                        from ZC10MMDG072 A 
                       JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "        WHERE A.ZSTATUS>'04' and  substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            sql += @"       GROUP BY A.MATNR) I ON F.WL_CODE=I.MATNR
                        LEFT JOIN 
                        (
                        select SUM(A.ZFHSL) CKSL,A.MATNR
                        from ZC10MMDG078 A 
                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "             WHERE A.ZSTATUS>'03' and  substr(A.ZCJRQ,1,6)='" + yearmonth + "'";
            sql += @"         GROUP BY A.MATNR) J ON F.WL_CODE=J.MATNR

                        LEFT JOIN 
                        (
                        select SUM(A.ZFHSL) CKSUMSL ,A.MATNR
                        from ZC10MMDG078 A 
                        JOIN WZ_ZDWZPZ B ON A.MATNR=B.WL_CODE";
            sql += "          WHERE A.ZSTATUS>'03' and substr(A.ZCJRQ,1,4)='" + year + "' AND CAST( substr(A.ZCJRQ,5,2) AS INT)<=  CAST('" + _month + "' AS INT)";
            sql += "         GROUP BY A.MATNR) K ON F.WL_CODE=K.MATNR ";

            if (!string.IsNullOrEmpty(MATNR))
            {
                sql += " where  F.MATNR like'%" + MATNR + "%'";
            }

            return db.GetDataTable(sql);
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
                   sql+="     where A.MATNR='"+MATNR+"'   and  substr(A.ZCJRQ,1,6)='"+MONTH+"'  group by B.WEMPF";
            return db.GetDataTable(sql);
        }
    }
}

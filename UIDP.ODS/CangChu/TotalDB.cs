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
        public DataSet getCRKDetail(string year,string month) {
            Dictionary<string, string> d = new Dictionary<string, string>();//以万为单位
            string sql = @" select DISTINCT B.CKH_NAME
                            from CONVERT_CKJE A 
                            join WZ_KCDD B on A.WERKS=B.DWCODE AND A.LGORT=B.KCDD_CODE
                            LEFT JOIN WZ_CRKL C ON DK_CODE=B.CKH  AND ";
            sql += " substr(C.ERDATE,1,4)='" + year + "' AND  substr(C.ERDATE,5,2)='" + month + "'";
            sql += "  WHERE substr(A.BUDAT_MKPF,1,4)='" + year + "' and substr(A.BUDAT_MKPF,5,2)='" + month + "'";
            sql += " UNION ";
            sql+= @"  select DISTINCT B.CKH_NAME
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
            sql +="   GROUP BY  B.CKH";
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
    }
}

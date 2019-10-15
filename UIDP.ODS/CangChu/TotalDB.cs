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
    }
}

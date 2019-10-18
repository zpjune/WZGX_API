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
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        public DataSet GetDRKInfo(string MATNR,string info,int page,int limit)
        {
            string PartSql  = " {0} SELECT {1} FROM {2}";
            string MainSql = "（SELECT a.ZDHTZD,a.MATKL,a.MATNR,b.PMNAME,d.MEINS,SUM(a.MENGE) AS MENGE,c.ERNAM,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1 FROM ZC10MMDG072 a " +
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATKL=d.MATKL" +
                " WHERE 1=1";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND a.MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql += " AND d.MATNR like'" + MATNR + "%'";
            }
            MainSql+= "GROUP BY a.ZDHTZD,a.MATKL,a.MATNR,b.PMNAME,d.MEINS,c.ERNAM,a.WERKS,c.NAME1)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql+ " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(1) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }


        public DataSet GetDCKInfo(string MATNR, string info, int page, int limit)
        {
            string PartSql = " {0} SELECT {1} FROM {2}";
            string MainSql = "（SELECT a.ZCKTZD,a.MATKL,a.MATNR,b.PMNAME,d.MEINS,SUM(a.ZFHSL) AS ZFHSL,c.ERNAM,a.WERKS,SUM(d.GESME)AS GESME,c.NAME1 FROM ZC10MMDG078 a " +
                " JOIN WZ_WLZ b ON a.MATKL=b.PMCODE" +
                " JOIN LFA1 c ON a.LIFNR=c.LIFNR" +
                " LEFT JOIN CONVERT_SWKC d ON a.MATKL=d.MATKL" +
                " WHERE 1=1";
            if (!string.IsNullOrEmpty(MATNR))
            {
                MainSql += " AND a.MATNR='" + MATNR + "'";
            }
            if (!string.IsNullOrEmpty(info))
            {
                MainSql += " AND d.MATNR like'" + MATNR + "%'";
            }
            MainSql += "GROUP BY a.ZCKTZD,a.MATKL,a.MATNR,b.PMNAME,d.MEINS,c.ERNAM,a.WERKS,c.NAME1)t";
            string DetailSql = string.Format(PartSql, " SELECT * FROM ( ", "ROWNUM rn, t.*", MainSql + " WHERE ROWNUM<" + ((page * limit) + 1) + ")WHERE rn>" + ((page - 1) * limit));
            string TotailSql = string.Format(PartSql, "", "COUNT(1) AS TOTAL", MainSql);
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("DetailSql", DetailSql);
            list.Add("TotailSql", TotailSql);
            return db.GetDataSet(list);
        }
    }
}

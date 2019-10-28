using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UIDP.ODS.CangChu;
using UIDP.UTILITY;

namespace UIDP.BIZModule.CangChu.Modules
{
    public class ZXKModule
    {
        ZXKDB db = new ZXKDB();
        public Dictionary<string,object> GetDRKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.GetDRKInfo(MATNR, info, FacCode,page, limit);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["total"] = ds.Tables[1].Rows[0]["TOTAL"];
                    r["items"] = ds.Tables[0];
                }
                else
                {
                    r["code"] = 2001;
                    r["message"] = "成功,但是没有数据";
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


        public Dictionary<string, object> GetDCKInfo(string MATNR, string info, string FacCode, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataSet ds = db.GetDCKInfo(MATNR, info, FacCode,page, limit);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["message"] = "成功";
                    r["total"] = ds.Tables[1].Rows[0]["TOTAL"];
                    r["items"] = ds.Tables[0];
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功,但是没有数据";
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
        /// <summary>
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE">大库编码</param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetFK_JYWZ(string DKCODE, string MATNR, string MATKL, int page, int limit)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                DataTable dt = db.GetFK_JYWZ(DKCODE, MATNR, MATKL);
                if (dt.Rows.Count > 0)
                {
                    r["code"] = 2000;
                    r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));//dt
                    r["message"] = "成功！";
                    r["total"] = dt.Rows.Count;
                }
                else
                {
                    r["code"] = 2000;
                    r["message"] = "成功！但是没有数据";
                    r["items"] = new DataTable();//dt
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
        /// <summary>
        /// 查询平面图内各库状态
        /// </summary>
        /// <param name="FacCode">工厂代码</param>
        /// <returns></returns>
        /***
         * 本段代码逻辑如下：
         * 1.根据查询SQL获取最大仓位号
         * 2.根据最大仓位号循环读取dt中的相关数据，采用二进制字符串的方式来表示状态是否存在，1表示存在，0表示不存在
         * 3.物资共有3中状态，所以在第一个循环当中还需要加第二个循环读取判断其他两个状态是否存在
         * 4.由于仓位号是字符串，所以对循环数i需要进行处理，i<10时将其变为01,02等
         ***/ 
        public Dictionary<string, object> GetFacStatus(string FacCode)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetFacStatus(FacCode);
                int MaxFacCode;
                int.TryParse(dt.Rows[dt.Rows.Count-1]["LG"].ToString(), out MaxFacCode);
                string StatusStr = string.Empty;
                for (int i = 1; i <= MaxFacCode; i++)
                {
                    string Stri = i.ToString();
                    while (Stri.Length < 2)
                    {
                        Stri = "0" + Stri;
                    }
                    for (int j = 1; j < 4; j++)
                    {
                        if (dt.Select("LG='" + Stri + "' AND Status=" + j).Length > 0)
                        {
                            StatusStr += "1";
                        }
                        else
                        {
                            StatusStr += "0";
                        }
                    }
                }
                r["code"] = 2000;
                r["message"] = "成功！";
                r["items"] = StatusStr;
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

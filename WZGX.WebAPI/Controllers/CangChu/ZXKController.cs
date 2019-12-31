using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule.CangChu.Modules;

namespace WZGX.WebAPI.Controllers.CangChu
{
    [Produces("application/json")]
    [Route("ZXK")]
    public class ZXKController : WebApiBaseController
    {
        ZXKModule ZXK = new ZXKModule();
        /// <summary>
        /// 查询待入库信息
        /// </summary>
        /// <param name="MATNR">物料编码</param>
        /// <param name="info">物料描述</param>
        /// /// <param name="FacCode">库存代码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetDRKInfo")]
        public IActionResult GetDRKInfo(string MATNR, string info, string FacCode, int page, int limit) => Ok(ZXK.GetDRKInfo(MATNR, info, FacCode, page, limit));

        /// <summary>
        /// 查询待出库信息
        /// </summary>
        /// <param name="MATNR">物料编码</param>
        /// <param name="info">物料描述</param>
        /// <param name="FacCode">库存代码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetDCKInfo")]
        public IActionResult GetDCKInfo(string MATNR, string info,string FacCode,int page, int limit) => Ok(ZXK.GetDCKInfo(MATNR, info, FacCode,page, limit));

        /// <summary>
        /// 积压物资第一层查询 按单位分组
        /// </summary>
        /// <param name="ISWZ">是否为物资公司</param>
        /// <param name="WERKS">工厂代码</param>
        /// <param name="DKCODE">大库代码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetTotalFK_JYWZ")]
        public IActionResult GetTotalFK_JYWZ(string ISWZ, string WERKS, string DKCODE, int page, int limit) => Ok(ZXK.GetTotalFK_JYWZ(ISWZ, WERKS, DKCODE, page, limit));

        /// <summary>
        /// 积压物资第二层查询 按大类分组
        /// </summary>
        /// <param name="ISWZ">是否为物资公司</param>
        /// <param name="WERKS">工厂代码</param>
        /// <param name="DKCODE">大库代码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetDLFK_JYWZ")]
        public IActionResult GetDLFK_JYWZ(string ISWZ, string WERKS, string DKCODE, int page, int limit) => Ok(ZXK.GetDLFK_JYWZ(ISWZ, WERKS, DKCODE, page, limit));
        /// <summary>
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetFK_JYWZ")]
        public IActionResult GetFK_JYWZ(string DLCODE,string ISWZ,string MEINS,string WERKS, string DKCODE, string MATNR, string MATKL, int page, int limit) => Ok(ZXK.GetFK_JYWZ(DLCODE,ISWZ, MEINS,WERKS, DKCODE,  MATNR,  MATKL, page, limit));
        /// <summary>
        /// 获取平面图各个点位状态展示字符串
        /// </summary>
        /// <param name="FacCode"></param>
        /// <returns></returns>
        [HttpGet("GetFacStatus")]
        public IActionResult GetFacStatus(string FacCode) => Ok(ZXK.GetFacStatus(FacCode));
        /// <summary>
        /// 重点物资储备查询-分库页面（停用）
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCB")]
        public IActionResult getZDWZCB(string DKCODE,string WERKS_NAME, string MATNR, string MATKL, int page, int limit) =>
            Ok(ZXK.getZDWZCB(DKCODE,WERKS_NAME,MATNR, MATKL, page, limit));
        /// <summary>
        /// 重点物资储备查询-分库页面
        /// </summary>
        /// <param name="DKCODE"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getDetailZDWZCBTOTAL")]
        public IActionResult getDetailZDWZCBTOTAL(string DKCODE, string MATNR, string MATKL,string MAKTX,int page, int limit) =>
           Ok(ZXK.getDetailZDWZCBTOTAL(DKCODE, MATNR, MATKL,MAKTX,page, limit));
        /// <summary>
        /// 重点物资储备查询-分库页面明细
        /// </summary>
        /// <param name="WERKS"></param>
        /// <param name="DKCODE"></param>
        /// <param name="WERKS_NAME"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getDetailZDWZCBTOTALDETAIL")]
        public IActionResult getDetailZDWZCBTOTALDETAIL(string WERKS,string DKCODE, string WERKS_NAME, string MATNR, string MATKL, int page, int limit) =>
           Ok(ZXK.getDetailZDWZCBTOTALDETAIL(WERKS,DKCODE, WERKS_NAME, MATNR, MATKL, page, limit));
        /// <summary>
        /// 重点物资出入库查询-分库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCRK")]
        public IActionResult getZDWZCRK(string DKCODE, string year,string MATNR) =>
            Ok(ZXK.getZDWZCRK(DKCODE, year, MATNR));
        /// <summary>
        /// 重点物资出入库明细-去向明细 分库页面
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MONTH"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getZDWZCRKDetail")]
        public IActionResult getZDWZCRKDetail(string DKCODE, string MATNR, string MONTH, int page, int limit) =>
            Ok(ZXK.getZDWZCRKDetail(DKCODE,MATNR, MONTH, page, limit));


        /// <summary>
        /// 获取重点物资物料数量
        /// </summary>
        /// <param name="DKCODE">大库编码</param>
        /// <returns></returns>
        [HttpGet("GetWLCount")]
        public IActionResult GetWLCount(string DKCODE) => Ok(ZXK.GetWLCount(DKCODE));

        /// <summary>
        /// 平面图详情查询
        /// </summary>
        /// <param name="LGPLA">编码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetStatusDetail")]
        public IActionResult GetStatusDetail(string LGPLA,string MATNR,string WERKS,int page, int limit) => Ok(ZXK.GetStatusDetail(LGPLA,MATNR,WERKS, page, limit));



        /// <summary>
        /// 平面图上半部第一层窗口
        /// </summary>
        /// <param name="LGPLA"></param>
        /// <returns></returns>
        [HttpGet("GetFloatWindowFirstInfo")]
        public IActionResult GetFloatWindowFirstInfo(string LGPLA) => Ok(ZXK.GetFloatWindowFirstInfo(LGPLA));
        /// <summary>
        /// 平面图上半部第二层窗口
        /// </summary>
        /// <param name="LGPLA">货位号</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetFloatWindowInfo")]
        public IActionResult GetFloatWindowInfo(string LGPLA, int page, int limit) => Ok(ZXK.GetFloatWindowInfo(LGPLA, page, limit));

        /// <summary>
        /// 平面图上半部第三层窗口
        /// </summary>
        /// <param name="LGPLA">货位号</param>
        /// <param name="DLCODE">大类编码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetGetFloatWindowDetailInfo")]
        public IActionResult GetGetFloatWindowDetailInfo(string LGPLA,string DLCODE,string LGORT,string MATNR,string MATKL,int page, int limit) => Ok(ZXK.GetGetFloatWindowDetailInfo(LGPLA,DLCODE, LGORT,MATNR,MATKL,page, limit));
    }
}
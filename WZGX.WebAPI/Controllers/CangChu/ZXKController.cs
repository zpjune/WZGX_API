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
        /// 查询积压物资-分库查询
        /// </summary>
        /// <param name="DKCODE"></param>
        /// <param name="MATNR"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetFK_JYWZ")]
        public IActionResult GetFK_JYWZ(string DKCODE, string MATNR, string MATKL, int page, int limit) => Ok(ZXK.GetFK_JYWZ(DKCODE,  MATNR,  MATKL, page, limit));

        [HttpGet("GetFacStatus")]
        public IActionResult GetFacStatus(string FacCode) => Ok(ZXK.GetFacStatus(FacCode));
        /// <summary>
        /// 重点物资储备查询-分库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCB")]
        public IActionResult getZDWZCB(string DKCODE, string WERKS_NAME, string MATNR, string MATKL, int page, int limit) =>
            Ok(ZXK.getZDWZCB(DKCODE,WERKS_NAME, MATNR, MATKL, page, limit));
        /// <summary>
        /// 重点物资出入库查询-分库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCRK")]
        public IActionResult getZDWZCRK(string DKCODE, string yearmonth,string MATNR, int page, int limit) =>
            Ok(ZXK.getZDWZCRK(DKCODE,yearmonth, MATNR,  page, limit));
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
        /// 平面图详情查询
        /// </summary>
        /// <param name="LGPLA">编码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetStatusDetail")]
        public IActionResult GetStatusDetail(string LGPLA, int page, int limit) => Ok(ZXK.GetStatusDetail(LGPLA, page, limit));
    }
}
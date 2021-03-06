﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule.CangChu.Modules;

namespace WZGX.WebAPI.Controllers.CangChu
{
    /// <summary>
    /// 总库存页面controller
    /// </summary>
    [Produces("application/json")]
    [Route("Total")]
    public class TotalController : WebApiBaseController
    {
        TotalModule md = new TotalModule();
        /// <summary>
        /// 查询总库存库存资金-饼图
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetKCZJ")]
        public IActionResult GetKCZJ() => Ok(md.GetKCZJ());
        /// <summary>
        /// 查询实物库存-总库页面第一层
        /// </summary>
        /// <param name="WERKS">工厂名称</param>
        /// <returns></returns>
        [HttpGet("GetSWKCDW")]
        public IActionResult GetSWKCDW(string ISWZ, string WERKS, string LGPLA, int page, int limit) =>
            Ok(md.GetSWKCDW(ISWZ, WERKS, LGPLA, page, limit));
        /// <summary>
        /// 查询实物库存-总库页面第二层
        /// </summary>
        /// <param name="WERKS"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetSWKCDL")]
        public IActionResult GetSWKCDL( string WERKS,string LGPLA, int page, int limit) =>
            Ok(md.GetSWKCDL( WERKS, LGPLA, page, limit));
        /// <summary>
        /// 查询实物库存-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("GetSWKC")]
        public IActionResult GetSWKC(string DLCODE,string ISWZ, string WERKS, string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit) => 
            Ok(md.GetSWKC( DLCODE,ISWZ,  WERKS, WERKS_NAME,  LGORTNAME,  MATNR,  MATKL,  page,  limit));

        /// <summary>
        /// 积压物资
        /// </summary>
        /// <param name="ISWZ">是否为物资公司</param>
        /// <param name="WERKS">工厂代码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetTotalJYWZ")]
        public IActionResult GetTotalJYWZ(string ISWZ, string WERKS, int page, int limit) => Ok(md.GetTotalJYWZ(ISWZ, WERKS, page, limit));

        /// <summary>
        /// 积压物资第二层查询 按大类分组
        /// </summary>
        /// <param name="ISWZ">是否为物资公司</param>
        /// <param name="WERKS">工厂代码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetDLJYWZ")]
        public IActionResult GetDLJYWZ(string ISWZ, string WERKS,int page, int limit) => Ok(md.GetDLJYWZ(ISWZ, WERKS, page, limit));

        /// <summary>
        /// 查询实物库存-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("GetJYWZ")]
        public IActionResult GetJYWZ(string DLCODE,string MEINS,string ISWZ, string WERKS, string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit) =>
            Ok(md.GetJYWZ(DLCODE, MEINS,ISWZ, WERKS, WERKS_NAME, LGORTNAME, MATNR, MATKL, page, limit));
        /// <summary>
        /// 总库存查询出入库金额 按年 按月分组统计
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("GetCRKJE")]
        public IActionResult GetCRKJE(string year,int ISWZ) =>
           Ok(md.GetCRKJE(year,ISWZ));
        /// <summary>
        /// 总库存-查询出库如明细
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("getCRKDetail")]
        public IActionResult getCRKDetail(string year,string month, int ISWZ, int page, int limit) =>
          Ok(md.getCRKDetail(year,month,  ISWZ,  page,  limit));
        /// <summary>
        /// 总库查询保管员工作量
        /// </summary>
        /// <param name="month"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getBGYGZL")]
        public IActionResult getBGYGZL(string month,string workerName, int page, int limit) =>
          Ok(md.getBGYGZL( month, workerName,page, limit));
        /// <summary>
        /// 总库存保管员工作量明细查询
        /// </summary>
        /// <param name="nianyue">年月</param>
        /// <param name="TZDType">1 入库单 2 出库单</param>
        /// <param name="workerCode">员工编号</param>
        [HttpGet("getBGYGZLDetail")]
        public IActionResult getBGYGZLDetail(string nianyue, string TZDType, string workerCode, int page, int limit) =>
          Ok(md.getBGYGZLDetail(nianyue,  TZDType,  workerCode, page, limit));
        /// <summary>
        /// 重点物资储备查询-左侧菜单
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCB")]
        public IActionResult getZDWZCB(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit) =>
            Ok(md.getZDWZCB(WERKS_NAME, LGORTNAME, MATNR, MATKL, page, limit));
        /// <summary>
        /// 重点物资储备查询-总库存
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCBTOTAL")]
        public IActionResult getZDWZCBTOTAL( string MATNR, string MATKL,string MAKTX,int page, int limit) =>
            Ok(md.getZDWZCBTOTAL( MATNR, MATKL, MAKTX,page, limit));
        /// <summary>
        /// 重点物资储备明细查询-总库存
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCBTOTALDETAIL")]
        public IActionResult getZDWZCBTOTALDETAIL(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit) =>
            Ok(md.getZDWZCBTOTALDETAIL(WERKS_NAME, LGORTNAME, MATNR, MATKL, page, limit));
        /// <summary>
        /// 重点物资出入库查询-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("getZDWZCRK")]
        public IActionResult getZDWZCRK(string year,  string MATNR) =>
            Ok(md.getZDWZCRK(year,MATNR));
        /// <summary>
        /// 重点物资出入库明细-去向明细
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MONTH"></param>
        /// <param name="MATKL"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("getZDWZCRKDetail")]
        public IActionResult getZDWZCRKDetail(string MATNR, string MONTH, string MATKL, int page, int limit) =>
            Ok(md.getZDWZCRKDetail(MATNR, MONTH, page, limit));
        [HttpGet("getZDWZPZ")]
        public IActionResult getZDWZPZ(string WL_NAME) =>
            Ok(md.getZDWZPZ(WL_NAME));


    }
}
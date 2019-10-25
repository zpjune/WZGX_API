﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIDP.BIZModule.CangChu.Modules;

namespace WZGX.WebAPI.Controllers.CangChu
{
    [Produces("application/json")]
    [Route("SWKC")]
    public class SWKCController : WebApiBaseController
    {
        SWKCModle SWKC = new SWKCModle();
        /// <summary>
        /// 单位物资查询接口
        /// </summary>
        /// <param name="WERKS">工厂代码</param>
        /// <param name="LGORT">地点代码</param>
        /// <param name="LGORT_NAME">地点名称</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页数据</param>
        /// <returns></returns>
        [HttpGet("GetFacInfo")]
        public IActionResult GetFacInfo(string WERKS, string LGORT, string LGORT_NAME, int page, int limit) => Ok(SWKC.GetFacInfo(WERKS, LGORT, LGORT_NAME, page, limit));
        /// <summary>
        /// 综合查询接口
        /// </summary>
        /// <param name="WERKS">工厂代码</param>
        /// <param name="LGORT">地点代码</param>
        /// <param name="LGORT_NAME">地点名称</param>
        /// <param name="MATNR">物料组编码</param>
        /// <param name="MAKTX">物料描述</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数据</param>
        /// <returns></returns>
        [HttpGet("GetCompositeInfo")]
        public IActionResult GetCompositeInfo(string WERKS, string LGORT, string LGORT_NAME, string MATNR, string MAKTX, int page, int limit) =>Ok(SWKC.GetCompositeInfo(WERKS, LGORT, LGORT_NAME, MATNR, MAKTX, page, limit));

        /// <summary>
        /// 分类查询父节点接口
        /// </summary>
        /// <param name="MATKL">物料组编码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <param name="level">节点等级</param>
        /// <returns></returns>
        [HttpGet("GetParentList")]
        public IActionResult GetParentList(string MATKL, int page, int limit, int level) => Ok(SWKC.GetParentList(MATKL, page, limit, level));
        /// <summary>
        /// 分类查询子节点接口
        /// </summary>
        /// <param name="code">当前节点编码</param>
        /// <param name="level">当前节点等级</param>
        /// <returns></returns>
        [HttpGet("GetChildrenList")]
        public IActionResult GetChildrenList(string code, int level) => Ok(SWKC.GetChildrenList(code, level));
    }
}
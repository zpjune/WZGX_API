using System;
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
        /// 查询实物库存-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("GetSWKC")]
        public IActionResult GetSWKC(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit) => 
            Ok(md.GetSWKC( WERKS_NAME,  LGORTNAME,  MATNR,  MATKL,  page,  limit));
        /// <summary>
        /// 查询实物库存-总库页面
        /// </summary>
        /// <param name="WERKS_NAME">工厂名称</param>
        /// <param name="LGORTNAME">库存地点名称</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATKL">物料组编码</param>
        /// <returns></returns>
        [HttpGet("GetJYWZ")]
        public IActionResult GetJYWZ(string WERKS_NAME, string LGORTNAME, string MATNR, string MATKL, int page, int limit) =>
            Ok(md.GetJYWZ(WERKS_NAME, LGORTNAME, MATNR, MATKL, page, limit));
        /// <summary>
        /// 总库存查询出入库金额 按年 按月分组统计
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("GetCRKJE")]
        public IActionResult GetCRKJE(string year) =>
           Ok(md.GetCRKJE(year));
        /// <summary>
        /// 总库存-查询出库如明细
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("getCRKDetail")]
        public IActionResult getCRKDetail(string year,string month) =>
          Ok(md.getCRKDetail(year,month));
    }
}
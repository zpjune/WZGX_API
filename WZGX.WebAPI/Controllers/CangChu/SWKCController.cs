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
        /// 单位物资导出接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExportFacInfo")]
        public IActionResult GetExportFacInfo() => Ok(SWKC.GetExportFacInfo());
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
        /// 综合查询导出接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExportCompositeInfo")]
        public IActionResult GetExportCompositeInfo() => Ok(SWKC.GetExportCompositeInfo());

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


        /// <summary>
        /// 物料查询
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="MAKTX"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetWLTotalInfo")]
        public IActionResult GetWLTotalInfo(string MATNR, string MAKTX, int page, int limit) => Ok(SWKC.GetWLTotalInfo(MATNR, MAKTX, page, limit));
        /// <summary>
        /// 物料查询导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExportWLTotalInfo")]
        public IActionResult GetExportWLTotalInfo() => Ok(SWKC.GetExportWLTotalInfo());

        /// <summary>
        /// 物料详情查询
        /// </summary>
        /// <param name="MATNR"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetWLDetail")]
        public IActionResult GetWLDetail(string MATNR, int page, int limit) => Ok(SWKC.GetWLDetail(MATNR,page, limit));
    }
}
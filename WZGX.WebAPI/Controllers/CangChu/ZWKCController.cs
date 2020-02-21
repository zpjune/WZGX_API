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
    [Route("ZWKC")]
    public class ZWKCController : WebApiBaseController
    {
        ZWKCModule ZWKC = new ZWKCModule();
        /// <summary>
        /// 分类查询父节点接口
        /// </summary>
        /// <param name="MATKL"></param>
        /// <param name="CODE"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("GetParentWLZList")]
        public IActionResult GetParentWLZList(string MATKL, string CODE, int level, int page, int limit) => Ok(ZWKC.GetParentWLZList(MATKL, CODE, level, page, limit));
        /// <summary>
        /// 获取子节点接口
        /// </summary>
        /// <param name="CODE">当前节点编码</param>
        /// <param name="level">当前节点等级</param>
        /// <returns></returns>
        [HttpGet("GetChildrenList")]
        public IActionResult GetChildrenList(string CODE, int level) => Ok(ZWKC.GetChildrenList(CODE, level));
        /// <summary>
        /// 单位资金查询接口
        /// </summary>
        /// <param name="BWKEY">工厂代码</param>
        /// <param name="BWKEY_NAME">工厂名称</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetFacMoney")]
        public IActionResult GetFacMoney(string BWKEY, string BWKEY_NAME, int page, int limit) => Ok(ZWKC.GetFacMoney(BWKEY, BWKEY_NAME, page, limit));
        /// <summary>
        /// 综合查询导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExportsFacMoney")]
        public IActionResult GetExportsFacMoney() => Ok(ZWKC.GetExportsFacMoney());
        /// <summary>
        /// 综合查询接口
        /// </summary>
        /// <param name="BWKEY">工厂代码</param>
        /// <param name="type">类型</param>
        /// <param name="CODE">编码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetCompositeInfo")]
        public IActionResult GetCompositeInfo(string BWKEY, int type, string CODE, int page, int limit) => Ok(ZWKC.GetCompositeInfo(BWKEY, type, CODE, page, limit));
        /// <summary>
        /// 综合查询导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetExportCompositeInfo")]
        public IActionResult GetExportCompositeInfo() => Ok(ZWKC.GetExportCompositeInfo());
    }
}
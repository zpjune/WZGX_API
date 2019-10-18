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
    }
}
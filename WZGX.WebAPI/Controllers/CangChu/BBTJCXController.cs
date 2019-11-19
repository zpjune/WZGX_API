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
    [Route("BBTJCX")]
    public class BBTJCXController : WebApiBaseController
    {
        BBTJCXModule BBTJ = new BBTJCXModule();
        /// <summary>
        /// 发出量统计查询
        /// </summary>
        /// <param name="Date">日期</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetFCLInfo")]
        public IActionResult GetFCLInfo(string Date, int page, int limit) => Ok(BBTJ.GetFCLInfo(Date, page, limit));

        /// <summary>
        /// 积压物资查询
        /// </summary>
        /// <param name="FacCode">仓库编码</param>
        /// <param name="page">页数</param>
        /// <param name="limit">每页条数</param>
        /// <returns></returns>
        [HttpGet("GetJYWZTJInfo")]
        public IActionResult GetJYWZTJInfo(string FacCode,string MATNR, int page, int limit) => Ok(BBTJ.GetJYWZTJInfo(FacCode, MATNR, page, limit));
    }
}
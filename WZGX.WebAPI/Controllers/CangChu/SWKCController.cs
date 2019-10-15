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
        [HttpGet("GetFacInfo")]
        public IActionResult GetFacInfo(string WERKS, string LGORT, string LGORT_NAME, int page, int limit) => Ok(SWKC.GetFacInfo(WERKS, LGORT, LGORT_NAME, page, limit));
    }
}
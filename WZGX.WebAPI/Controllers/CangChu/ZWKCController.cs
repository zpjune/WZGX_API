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
        [HttpGet("GetParentWLZList")]
        public IActionResult GetParentWLZList(string MATKL, string CODE, int level, int page, int limit) => Ok(ZWKC.GetParentWLZList(MATKL, CODE, level, page, limit));
        [HttpGet("GetChildrenList")]
        public IActionResult GetChildrenList(string CODE, int level) => Ok(ZWKC.GetChildrenList(CODE, level));
        [HttpGet("GetFacMoney")]
        public IActionResult GetFacMoney(string BWKEY, string BWKEY_NAME, int page, int limit) => Ok(ZWKC.GetFacMoney(BWKEY, BWKEY_NAME, page, limit));
        [HttpGet("GetCompositeInfo")]
        public IActionResult GetCompositeInfo(string BWKEY, int type, string CODE, int page, int limit) => Ok(ZWKC.GetCompositeInfo(BWKEY, type, CODE, page, limit));
    }
}
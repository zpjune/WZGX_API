using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule.CangChu.Modules;

namespace WZGX.WebAPI.Controllers.CangChu
{
    [Produces("application/json")]
    [Route("CRKWH")]
    public class CRKWHController : Controller
    {
        CRKWHModule CRKWH = new CRKWHModule();
        [HttpGet("GetCRKInfo")]
        public IActionResult GetCRKInfo(string DK_CODE, string ERDATE, int page, int limit) => Ok(CRKWH.GetCRKInfo(DK_CODE, ERDATE, page, limit));
        [HttpPost("CreateCRKInfo")]
        public IActionResult CreateCRKInfo([FromBody]JObject value) => Ok(CRKWH.CreateCRKInfo(value.ToObject<Dictionary<string, object>>()));

        [HttpPost("UpdateCRKInfo")]
        public IActionResult UpdateCRKInfo([FromBody]JObject value) => Ok(CRKWH.UpdateCRKInfo(value.ToObject<Dictionary<string, object>>()));

        [HttpPost("DelCRKInfo")]
        public IActionResult DelCRKInfo([FromBody]JObject value) => Ok(CRKWH.DelCRKInfo(value.Value<string>("ID")));


    }
}
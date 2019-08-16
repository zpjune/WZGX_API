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
    [Route("KCDDWH")]
    public class KCDDWHController : Controller
    {
        KCDDWHModule kCDDWH = new KCDDWHModule();
        [HttpGet("GetKCDDInfo")]
        public IActionResult GetKCDDInfo(string KCDD_CODE, string KCDD_NAME, int limit, int page) => Ok(kCDDWH.GetKCDDInfo(KCDD_CODE, KCDD_NAME,limit, page));

        [HttpPost("CreateKCDDInfo")]
        public IActionResult CreateKCDDInfo([FromBody]JObject value) => Ok(kCDDWH.CreateKCDDInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpPost("EditKCDDInfo")]
        public IActionResult EditKCDDInfo([FromBody]JObject value) => Ok(kCDDWH.EditKCDDInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpPost("DelKCDDinfo")]
        public IActionResult DelKCDDinfo([FromBody]JObject value) => Ok(kCDDWH.DelKCDDInfo(value.ToObject<Dictionary<string, object>>()));
    }
}
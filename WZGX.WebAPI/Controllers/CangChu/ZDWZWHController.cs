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
    [Route("ZDWZWH")]
    public class ZDWZWHController : WebApiBaseController
    {
        ZDWZWHModule zDWZWH = new ZDWZWHModule();

        [HttpGet("GetZDWZWHInfo")]
        public IActionResult GetZDWZWHInfo(string WL_LOCATIONCODE, string WL_CODE, int limit, int page) => Ok(zDWZWH.GetZDWZWHInfo(WL_LOCATIONCODE, WL_CODE, limit, page));

        [HttpGet("CreateZDWZWHInfo")]
        public IActionResult CreateZDWZWHInfo([FromBody]JObject value) => Ok(zDWZWH.CreateZDWZWHInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpGet("EditZDWZWHInfo")]
        public IActionResult EditZDWZWHInfo([FromBody]JObject value) => Ok(zDWZWH.EditZDWZWHInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpGet("DelZDWZWHInfo")]
        public IActionResult DelZDWZWHInfo([FromBody]JObject value) => Ok(zDWZWH.DelZDWZWHInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpGet("GetKCDDInfo")]
        public IActionResult GetKCDDInfo() => Ok(zDWZWH.GetKCDDInfo());

    }
}
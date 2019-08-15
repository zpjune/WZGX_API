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
    [Route("GCWH")]
    public class GCWHController : Controller
    {
        GCWHModule gCWHModule = new GCWHModule();
        [HttpGet("GetGCInfo")]
        public IActionResult GetGCInfo(string DW_CODE,string DW_NAME,int limit,int page)=> Ok(gCWHModule.GetGCInfo(DW_CODE, DW_NAME, limit, page));

        [HttpPost("CreateGCInfo")]
        public IActionResult CreateGCInfo([FromBody]JObject value)=>Ok(gCWHModule.CreateGCInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpPost("EditGCInfo")]
        public IActionResult EditGCInfo([FromBody]JObject value)=> Ok(gCWHModule.EditGCInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpPost("DelGCinfo")]
        public IActionResult DelGCinfo([FromBody]JObject value)=> Ok(gCWHModule.DelGCInfo(value.ToObject<Dictionary<string, object>>()));
    }
}
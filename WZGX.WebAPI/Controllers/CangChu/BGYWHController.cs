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
    [Route("BGYWH")]
    public class BGYWHController : Controller
    {
        BGYModule bGYModule = new BGYModule();

        [HttpGet("GetBGYInfo")]
        public IActionResult GetBGYInfo(string WORKER_CODE, string WORKER_NAME, string GC_CODE,string WORKER_DP, int limit, int page)=> Ok(bGYModule.GetBGYInfo(WORKER_CODE, WORKER_NAME, WORKER_DP, limit, page));

        [HttpPost("CreateBGYInfo")]
        public IActionResult CreateBGYInfo([FromBody]JObject value) => Ok(bGYModule.CreateBGYInfo(value.ToObject<Dictionary<string, object>>()));

        [HttpPost("EditBGYInfo")]
        public IActionResult EditBGYInfo([FromBody]JObject value)=> Ok(bGYModule.EditBGYInfo(value.ToObject<Dictionary<string, object>>()));
        [HttpPost("DelBGYInfo")]
        public IActionResult DelBGYInfo([FromBody]JObject value)=> Ok(bGYModule.DelBGYInfo(value.ToObject<Dictionary<string, object>>()));

        [HttpGet("GetGCInfo")]
        public IActionResult GetGCInfo()=>Ok(bGYModule.GetGCInfo());

        [HttpGet("GetCKHInfo")]
        public IActionResult GetCKHInfo() => Ok(bGYModule.GetCKHInfo());
    }
}
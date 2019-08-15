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
        public IActionResult GetBGYInfo(string WORKERCODE, string WORKERNAME, string WORKER_DP, int limit, int page)=> Ok(bGYModule.GetBGYInfo(WORKERCODE, WORKERNAME, WORKER_DP, limit, page));
        public IActionResult CreateBGYInfo([FromBody]JObject value) => Ok(bGYModule.CreateBGYInfo(value.ToObject<Dictionary<string, object>>()));
        public IActionResult EditBGYInfo([FromBody]JObject value)=> Ok(bGYModule.EditBGYInfo(value.ToObject<Dictionary<string, object>>()));
        public IActionResult DelBGYInfo([FromBody]JObject value)=> Ok(bGYModule.DelBGYInfo(value.ToObject<Dictionary<string, object>>()));

        public IActionResult GetGCInfo()=>Ok(bGYModule.GetGCInfo());
    }
}
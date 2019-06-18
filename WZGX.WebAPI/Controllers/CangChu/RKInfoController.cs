using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule.CangChu;

namespace WZGX.WebAPI.Controllers.CangChu
{
    [Produces("application/json")]
    [Route("RKInfo")]
    public class RKInfoController : Controller
    {
        RKInfoModel RK = new RKInfoModel();

        [HttpGet("GetRKInfo")]
        public IActionResult GetRKInfo(int page, int limit, string RKTime, string LocationNumber)
        {
            return Json(RK.GetRKInfo(page, limit, RKTime, LocationNumber));
        }

        [HttpPost("CreateRKInfo")]
        public IActionResult CreateRKInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            return Json(RK.CreateRKInfo(d));
        }
        [HttpPost("UpdateRKInfo")]
        public IActionResult UpdateRKInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            return Json(RK.UpdateRKInfo(d));
        }
        [HttpPost("DeleteRKInfo")]
        public IActionResult DeleteRKInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            return Json(RK.DeleteRKInfo(d));
        }
    }
}
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
    [Route("CKInfo")]
    public class CKInfoController : Controller
    {
        CKInfoModule CK = new CKInfoModule();

        [HttpGet("GetCKInfo")]
        public IActionResult GetCKInfo(int page, int limit, string CKTime, string LocationNumber)
        {
            return Json(CK.GetRKInfo(page, limit, CKTime, LocationNumber));
        }

        [HttpPost("CreateCKInfo")]
        public IActionResult CreateRKInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            return Json(CK.CreateCKInfo(d));
        }
        [HttpPost("UpdateCKInfo")]
        public IActionResult UpdateRKInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            return Json(CK.UpdateCKInfo(d));
        }
        [HttpPost("DeleteCKInfo")]
        public IActionResult DeleteRKInfo([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            return Json(CK.DeleteCKInfo(d));
        }
    }
}
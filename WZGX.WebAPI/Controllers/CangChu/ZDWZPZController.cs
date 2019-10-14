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
    [Route("ZDWZPZ")]
    public class ZDWZPZController : WebApiBaseController
    {
        ZDWZPZModel zDWZPZModel = new ZDWZPZModel();
        [HttpGet("GetZDWZPZInfo")]
        public IActionResult GetZDWZPZInfo(string WLZ_CODE, string WL_CODE,string WL_NAME, int limit, int page) => Ok(zDWZPZModel.GetZDWZPZInfo(WLZ_CODE, WL_CODE,WL_NAME,limit, page));

        [HttpPost("CreateZDWZPZInfo")]
        public IActionResult CreateZDWZPZInfo([FromBody]JObject value) => Ok(zDWZPZModel.CreateZDWZPZInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpPost("EditZDWZPZInfo")]
        public IActionResult EditZDWZPZInfo([FromBody]JObject value) => Ok(zDWZPZModel.EditZDWZPZInfo(value.ToObject<Dictionary<string, string>>()));

        [HttpPost("DelZDWZPZinfo")]
        public IActionResult DelZDWZPZinfo([FromBody]JObject value) => Ok(zDWZPZModel.DelZDWZPZInfo(value.ToObject<Dictionary<string, object>>()));

        [HttpGet("GetPMCODE")]
        public IActionResult GetPMCODE() => Ok(zDWZPZModel.GetPMCODE());

        [HttpGet("GetParentNode")]
        public IActionResult GetParentNode() => Ok(zDWZPZModel.CreateParentNode());


        [HttpGet("GetChildrenNode")]
        public IActionResult GetChildrenNode(string flagID, string DLCODE, string ZLCODE, string XLCODE) => Ok(zDWZPZModel.CreateChildrenNode(flagID,DLCODE,ZLCODE,XLCODE));

        [HttpGet("GetEditParentNode")]
        public IActionResult GetEditParentNode(string PMCODE) => Ok(zDWZPZModel.GetEditParentNode(PMCODE));
    }
}
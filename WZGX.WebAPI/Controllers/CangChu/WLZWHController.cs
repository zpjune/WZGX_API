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
    [Route("WLZWH")]
    public class WLZWHController : WebApiBaseController
    {
        WLZWZModule wLZWZModule = new WLZWZModule();

        [HttpGet("GetParentWLZList")]
        public IActionResult GetParentWLZList(string WLZCODE, string WLZNAME, int limit, int page)=> Ok(wLZWZModule.GetParentWLZList(WLZCODE, WLZNAME, limit, page));

        [HttpGet("GetChildrenWLZList")]
        public IActionResult GetChildrenWLZList(string DLCODE,string ZLCODE,string XLCODE,string FlagID,int level)=> Ok(wLZWZModule.GetChildrenWLZList(DLCODE, ZLCODE, XLCODE, FlagID, level));

        [HttpPost("editNode")]
        public IActionResult editNode([FromBody]JObject value)=> Ok(wLZWZModule.editNode(value.ToObject<Dictionary<string, object>>()));

        [HttpGet("getDLOptions")]
        public IActionResult getDLOptions()=>Ok(wLZWZModule.getDLOptions());
        [HttpGet("getZLOptions")]
        public IActionResult getZLOptions(string DLCODE)=> Ok(wLZWZModule.getZLOptions(DLCODE));
        [HttpGet("getXLOptions")]
        public IActionResult getxLOptions(string DLCODE,string ZLCODE)=> Ok(wLZWZModule.getXLOptions(DLCODE, ZLCODE));

        [HttpGet("delNode")]
        public IActionResult delNode(string ID)=>Ok(wLZWZModule.delNode(ID));

        [HttpPost("createNode")]
        public IActionResult createNode([FromBody]JObject value)=> Ok(wLZWZModule.createNode(value.ToObject<Dictionary<string, object>>()));
    }
}
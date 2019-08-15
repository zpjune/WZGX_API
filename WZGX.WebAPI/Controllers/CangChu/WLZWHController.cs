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
        public IActionResult GetParentWLZList(string WLZCODE, string WLZNAME, int limit, int page)
        {
            return Ok(wLZWZModule.GetParentWLZList(WLZCODE, WLZNAME, limit, page));
        }

        [HttpGet("GetChildrenWLZList")]
        public IActionResult GetChildrenWLZList(string DLCODE,string ZLCODE,string XLCODE,int level)
        {
            return Ok(wLZWZModule.GetChildrenWLZList(DLCODE,ZLCODE,XLCODE,level));
        }

        [HttpPost("editNode")]
        public IActionResult editNode([FromBody]JObject value)
        {
            return Ok(wLZWZModule.editNode(value.ToObject<Dictionary<string, object>>()));
        }

        [HttpGet("getOptions")]
        public IActionResult getOptions()
        {
            return Ok(wLZWZModule.getOptions());
        }

        [HttpGet("delNode")]
        public IActionResult delNode(string id)
        {
            return Ok(wLZWZModule.delNode(id));
        }

        [HttpPost("createNode")]
        public IActionResult createNode([FromBody]JObject value)
        {
            return Ok(wLZWZModule.createNode(value.ToObject<Dictionary<string, object>>()));
        }
    }
}
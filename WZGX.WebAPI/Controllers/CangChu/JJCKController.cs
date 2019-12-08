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
    [Route("JJCK")]
    public class JJCKController : WebApiBaseController
    {
        JJCKModule JJCK = new JJCKModule();
        /// <summary>
        /// 紧急出库单查询
        /// </summary>
        /// <param name="CODE">单号</param>
        /// <param name="MATNR">物料编码</param>
        /// <param name="MATNX">物料描述</param>
        /// <param name="MATNX">字典表父节点code</param>
        /// <param name="userid">登录人id</param>
        /// <param name="type">查询类型，0为非审批查询，1为审批待办，2为已办</param>
        /// <param name="limit">每页条数</param>
        /// <param name="page">页数</param>
        /// <returns></returns>
        [HttpGet("GetCKInfo")]
        public IActionResult GetCKInfo(string CODE, string MATNR, string MATNX, string ParentCode, string userid, int type, int limit, int page) => Ok(JJCK.GetRKInfo(CODE, MATNR, MATNX, ParentCode, userid, type, limit, page));
        /// <summary>
        /// 创建紧急出库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [HttpPost("CreateJJCKInfo")]
        public IActionResult CreateJJCKInfo([FromBody]JObject value) => Ok(JJCK.CreateJJCKInfo(value.ToObject<Dictionary<string, object>>()));
        /// <summary>
        /// 修改紧急出库单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("UpdateJJCKInfo")]
        public IActionResult UpdateJJCKInfo([FromBody]JObject value) => Ok(JJCK.UpdateJJCKInfo(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 删除紧急出库单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("DelJJCKInfo")]
        public IActionResult DelJJCKInfo([FromBody]JObject value) => Ok(JJCK.DelJJCKInfo(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 发起紧急出库流程
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("StartProcess")]
        public IActionResult StartProcess([FromBody]JObject value) => Ok(JJCK.StartProcess(value.ToObject<Dictionary<string, object>>()));

        /// <summary>
        /// 紧急出库撤回
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Recall")]
        public IActionResult Recall([FromBody]JObject value) => Ok(JJCK.Recall(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 紧急出库审批
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Approval")]
        public IActionResult Approval([FromBody]JObject value) => Ok(JJCK.Approval(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 紧急出库数据从ERP再次录出到系统中时，保管员调用此方法将模型表和出库表中的数据置为无效
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("CancelRK")]
        public IActionResult CancelRK([FromBody]JObject value) => Ok(JJCK.CancelRK(value.ToObject<Dictionary<string, object>>()));

        [HttpGet("GetOrgInfo")]
        public IActionResult GetOrgInfo() => Ok(JJCK.GetOrgInfo());

        [HttpGet("GetKCDDInfo")]
        public IActionResult GetKCDDInfo(string orgCode) => Ok(JJCK.GetKCDDInfo(orgCode));

        /// <summary>
        /// 保管员更新表单接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("BGYUpdate")]
        public IActionResult BGYUpdate([FromBody]JObject value) => Ok(JJCK.BGYUpdate(value.ToObject<Dictionary<string, object>>()));
        /// <summary>
        /// 保管员提交表单接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("BGYSendForm")]
        public IActionResult BGYSendForm([FromBody]JObject value) => Ok(JJCK.BGYSendForm(value.ToObject<Dictionary<string, object>>()));
    }
}
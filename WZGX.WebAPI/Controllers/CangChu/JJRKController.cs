using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule.CangChu.Modules;
using UIDP.BIZModule.Modules;

namespace WZGX.WebAPI.Controllers.CangChu
{
    [Produces("application/json")]
    [Route("JJRK")]
    public class JJRKController : WebApiBaseController
    {
        JJRKModule JJRK = new JJRKModule();
        /// <summary>
        /// 紧急入库单查询
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
        [HttpGet("GetRKInfo")]
        public IActionResult GetRKInfo(string CODE, string MATNR, string MATNX, string ParentCode, string userid, int type, int limit, int page) => Ok(JJRK.GetRKInfo(CODE, MATNR, MATNX, ParentCode, userid, type,limit,page));
        /// <summary>
        /// 创建紧急入库单
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        [HttpPost("CreateJJRKInfo")]
        public IActionResult CreateJJRKInfo([FromBody]JObject value) => Ok(JJRK.CreateJJRKInfo(value.ToObject<Dictionary<string, object>>()));
        /// <summary>
        /// 修改紧急入库单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("UpdateJJRKInfo")]
        public IActionResult UpdateJJRKInfo([FromBody]JObject value) => Ok(JJRK.UpdateJJRKInfo(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 删除紧急入库单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("DelJJRKInfo")]
        public IActionResult DelJJRKInfo([FromBody]JObject value) => Ok(JJRK.DelJJRKInfo(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 发起紧急入库流程
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("StartProcess")]
        public IActionResult StartProcess([FromBody]JObject value) => Ok(JJRK.StartProcess(value.ToObject<Dictionary<string, object>>()));

        /// <summary>
        /// 紧急入库撤回
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Recall")]
        public IActionResult Recall([FromBody]JObject value) => Ok(JJRK.Recall(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 紧急入库审批
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Approval")]
        public IActionResult Approval([FromBody]JObject value) => Ok(JJRK.Approval(value.ToObject<Dictionary<string, object>>()));


        /// <summary>
        /// 紧急入库数据从ERP再次录入到系统中时，保管员调用此方法将模型表和入库表中的数据置为无效
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("CancelRK")]
        public IActionResult CancelRK([FromBody]JObject value) => Ok(JJRK.CancelRK(value.ToObject<Dictionary<string, object>>()));
        /// <summary>
        /// 获取组织机构接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrgInfo")]
        public IActionResult GetOrgInfo() => Ok(JJRK.GetOrgInfo());

        /// <summary>
        /// 获取库存地点接口
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        [HttpGet("GetKCDDInfo")]
        public IActionResult GetKCDDInfo(string orgCode) => Ok(JJRK.GetKCDDInfo(orgCode));
        /// <summary>
        /// 保管员更新表单接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("BGYUpdate")]
        public IActionResult BGYUpdate([FromBody]JObject value) => Ok(JJRK.BGYUpdate(value.ToObject<Dictionary<string, object>>()));
        /// <summary>
        /// 保管员提交表单接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("BGYSendForm")]
        public IActionResult BGYSendForm([FromBody]JObject value) => Ok(JJRK.BGYSendForm(value.ToObject<Dictionary<string, object>>()));

    }
}
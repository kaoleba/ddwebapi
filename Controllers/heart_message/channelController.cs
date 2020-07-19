using Dapper;
using DDWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class channelController : ControllerBase
    {
        private DBHelper db = new DBHelper();

        private static UserInfo SystemInfo = new UserInfo("10003641", "张银萍");//负责人
        private static UserInfo HeShujiInfo = new UserInfo("10016138", "何祥成");//何书记
        private static UserInfo FuChuzhInfo = new UserInfo("10010399", "李宪寅");//副处长
        private static UserInfo ChuzhInfo = new UserInfo("10015926", "王学兵");//处长
        private static string SystemUrl = "http://221.2.76.14:8055/#/dd_channel/syslist";//民生通道办理Url
        private static string ReplyUrl = "http://221.2.76.14:8055/#/channel_todo";//民生回复Url

        // GET api/channel
        [HttpGet]
        [EnableCors("any")]
        public ActionResult<IEnumerable<heart_message>> Get(int? page, string state, string key)
        {
            try
            {
                if (page is null)
                    return null;
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select * from heart_message  where 1=1 ");

                if (!string.IsNullOrEmpty(state))
                {
                    sql.Append(" and state=?state");
                    pars.Add("state", state);
                }
                if (!string.IsNullOrEmpty(key))
                {
                    sql.Append(" and title like ?key or message like ?key");
                    pars.Add("key", "%" + key + "%");
                }
                return db.GetPageList<heart_message>(sql.ToString(), pars, "createdate", "desc", Convert.ToInt32(page), 20);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取留言信息异常：" + ex.Message);
                return null;
            }
        }

        [HttpGet]
        [EnableCors("any")]
        [Route("GetMessage")]
        public ActionResult<IEnumerable<heart_message>> GetMessage(string Code)
        {
            try
            {
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select * from heart_message  where state in (0,1) ");

                if (!string.IsNullOrEmpty(Code))
                {
                    sql.Append(" and currentcode=?Code");
                    pars.Add("Code", Code);
                }
                sql.Append(" order by createdate desc");
                return db.GetList<heart_message>(sql.ToString(), pars);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取留言信息异常：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("any")]
        [Route("GetFlow")]
        public ActionResult<FlowInfo> GetFlow(string id)
        {
            try
            {
                FlowInfo flowinfo = new FlowInfo();
                heart_message hesave = db.GetEntityById<heart_message>(id);
                DBHelper ssdb = new DBHelper("MSSQLCon");
                var dept = ssdb.GetEntity<DeptInfo>($"select top 1 Name,UserCode,(select ABBREVIATION+'-'+deptName from msgcenter_dept b where a.DeptCode=b.DeptCode) Dept,DingTalk from msgcenter_person a where UserCode = '{hesave.currentcode}'");
                List<heart_flow> list = db.GetList<heart_flow>($"select * from heart_flow where messageid='{id}' order by createdate");
                flowinfo.deptInfo = dept;
                flowinfo.flowlist = list;
                return flowinfo;
            }
            catch
            {
                return null;
            }
        }

        // GET  api/proposal/5
        [HttpGet("{id}")]
        [EnableCors("any")]
        public ActionResult<heart_message> Get(string id)
        {
            var pro = db.GetEntityById<heart_message>(id);
            return pro;
        }

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("any")]
        [Route("GetDept")]
        public ActionResult<DeptInfo> GetDept(string id)
        {
            return GetDeptInfo(id);
        }

        public DeptInfo GetDeptInfo(string id)
        {
            DBHelper ssdb = new DBHelper("MSSQLCon");
            var dept = ssdb.GetEntity<DeptInfo>($"select top 1 Name,UserCode,(select ABBREVIATION+'-'+deptName from msgcenter_dept b where a.DeptCode=b.DeptCode) Dept,DingTalk,Phone from msgcenter_person a where DingTalk = '{id}'");
            return dept;
        }

        /// <summary>
        /// 保存留言信息
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("PostMessage")]
        public string PostMessage(heart_message he)
        {
            try
            {
                he.state = 0;
                he.origin = "PC";
                if (he.createdate == null)
                    he.createdate = DateTime.Now;
                he.updatedate = he.createdate;
                he.currentcode = SystemInfo.Code;
                he.message = tengdaHelper.ReplaceHtmlTag(he.message);
                db.Insert<heart_message>(he);
                //给管理员发送通知消息
                var res = DDHelper.SendMSTDMessage(SystemInfo.Code, "民生通道审批提醒", "您有新的民生通道消息需要处理", SystemUrl);
   
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("留言异常：" + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 保存留言信息
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SaveMessage")]
        public string SaveMessage(heart_message he)
        {
            try
            {
                if (string.IsNullOrEmpty(he.id))
                {
                    he.Create();
                    if (string.IsNullOrEmpty(he.ip))
                    {
                        he.ip = HttpContext.GetClientUserIp();
                    }
                    he.currentcode = SystemInfo.Code;

                    //插入山能接口
                    he.id=tengdaHelper.NewMessange(he);
                    db.Insert<heart_message>(he);
                    //给管理员发送通知消息
                    var res = DDHelper.SendMSTDMessage(SystemInfo.Code, "民生通道审批提醒", "您有新的民生通道消息需要处理", SystemUrl);
                }
                else
                {
                    db.Update<heart_message>(he);
                }
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("留言异常：" + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 领导审批
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SaveFlow")]
        public string SaveFlow(heart_message he)
        {
            var tran = db.BeginTransaction();
            try
            {
                heart_message hesave = db.GetEntityById<heart_message>(he.id);
                heart_flow hf = new heart_flow();
                hf.Create();
                hf.messageid = he.id;

                if (hesave.nextcode == "lxm")
                {
                    hf.code = FuChuzhInfo.Code;
                    hf.name = FuChuzhInfo.Name;
                    hesave.currentcode = ChuzhInfo.Code;
                    hesave.nextcode = "wxb";
                }
                else if (hesave.nextcode == "wxb")
                {
                    hf.code = ChuzhInfo.Code;
                    hf.name = ChuzhInfo.Name;
                    if (hesave.type == "纪律作风")
                    {
                        hesave.currentcode = HeShujiInfo.Code;
                        hesave.nextcode = "hsjflow";
                    }
                    else
                    {
                        hesave.currentcode = SystemInfo.Code;
                        hesave.nextcode = "pub";
                    }
                }
                else if (hesave.nextcode == "hsjflow")
                {
                    hf.code = HeShujiInfo.Code;
                    hf.name = HeShujiInfo.Name;
                    hesave.currentcode = SystemInfo.Code;
                    hesave.nextcode = "pub";
                }
                else if (hesave.nextcode == "pub")
                {
                    hf.code = SystemInfo.Code;
                    hf.name = SystemInfo.Name;
                    hesave.nextcode = "";
                    hesave.currentcode = "";
                    hesave.publicdate = DateTime.Now;
                    hesave.state = 2;
                }
                if (hesave.state == 2)
                {
                    //调用山能接口
                    tengdaHelper.ReplyMessage(hesave);
                }
                db.Update<heart_message>(hesave, tran);
                db.Insert<heart_flow>(hf, tran);
                db.Commit();
                if (hesave.state != 2)
                {
                    if (hesave.nextcode == "pub")
                    {
                        DDHelper.SendMSTDMessage(hesave.currentcode, "民生通道处理", $"您提交的民生通道承办已通过审批，请您发布", SystemUrl);
                    }
                    else
                    {
                        DDHelper.SendMSTDMessage(hesave.currentcode, "民生通道处理", $"{ hf.name }提交的民生通道承办，请您批阅", SystemUrl);
                    }
                }
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("发布异常：" + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 发送领导审批
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SendFlow")]
        public string SendFlow(heart_message he)
        {
            var tran = db.BeginTransaction();
            try
            {
                heart_message hesave = db.GetEntityById<heart_message>(he.id);
                hesave.currentcode = FuChuzhInfo.Code;
                heart_flow hf = new heart_flow();
                hf.Create();
                hf.messageid = he.id;
                hf.code = SystemInfo.Code;
                hf.name = SystemInfo.Name;
                hesave.reply = he.reply;
                hesave.nextcode = "lxm";
                db.Update<heart_message>(hesave, tran);
                db.Insert<heart_flow>(hf, tran);
                db.Commit();
                DDHelper.SendMSTDMessage(FuChuzhInfo.Code, "民生通道处理", $"{ hf.name }提交的民生通道承办，请您批阅", SystemUrl);

                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("留言异常：" + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 保存回复信息
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SaveReply")]
        public string SaveReply(heart_message he)
        {
            var tran = db.BeginTransaction();
            try
            {
                heart_message hesave = db.GetEntityById<heart_message>(he.id);
                hesave.currentcode = SystemInfo.Code;
                heart_flow hf = new heart_flow();
                hf.Create();
                hf.messageid = he.id;
                hf.code = hesave.code;
                hf.name = hesave.username;
                hesave.replymessage = he.replymessage;
                hesave.replaydate = DateTime.Now;
                hesave.nextcode = "sys";
                //给处理者发送待办信息            
                var res = DDHelper.UpdateReplyMessage(hesave.dingid, hesave.replyid);

                if (res.errorMsg == "")
                {
                    db.Update<heart_message>(hesave, tran);
                    db.Insert<heart_flow>(hf, tran);
                    db.Commit();
                    DDHelper.SendMSTDMessage(hesave.currentcode, "民生通道处理", $"{ hf.name }提交的民生通道承办，请您批阅", SystemUrl);
                }
                else
                {
                    throw new Exception("推送待办异常：" + res.errorMsg);
                }
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("留言异常：" + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 发送回复待办信息
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SendReply")]
        public string SendReply(heart_message he)
        {
            var tran = db.BeginTransaction();
            try
            {
                heart_message hesave = db.GetEntityById<heart_message>(he.id);

                heart_flow hf = new heart_flow();
                hf.Create();
                hf.messageid = he.id;
                if (hesave.nextcode == "hsj")
                {
                    hf.code = HeShujiInfo.Code;
                    hf.name = HeShujiInfo.Name;
                }
                else
                {
                    hesave.type = he.type;
                    hf.code = SystemInfo.Code;
                    hf.name = SystemInfo.Name;
                }

                hesave.currentcode = he.code;
                hesave.nextcode = "";
                hesave.username = he.username;
                hesave.dingid = he.dingid;
                hesave.code = he.code;
                hesave.convertmessage = he.convertmessage;
                hesave.dept = he.dept;
                hesave.state = 1;
                //给处理者发送待办信息            
                var res = DDHelper.SendReplyMessage(hesave.dingid, "民生通道处理", "您有新的民生通道消息需要处理", ReplyUrl + "?id=" + he.id);

                if (res.errorMsg == "")
                {
                    hesave.replyid = res.content.ToString();
                    db.Update<heart_message>(hesave, tran);
                    db.Insert<heart_flow>(hf, tran);
                    db.Commit();
                    DDHelper.SendMSTDMessage(hesave.code, "民生通道处理", "您有新的民生通道消息需要处理", ReplyUrl + "?id=" + he.id);

                }
                else
                {
                    throw new Exception("推送待办异常：" + res.errorMsg);
                }
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("留言异常：" + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 纪律作风问题通知何书记
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SendHe")]
        public string SendHe(heart_message he)
        {
            var tran = db.BeginTransaction();
            try
            {
                he = db.GetEntityById<heart_message>(he.id);
                if (he == null)
                {
                    throw new Exception("留言信息获取异常");
                }
                he.state = 1;
                he.currentcode = HeShujiInfo.Code;
                he.nextcode = "hsj";
                he.type = "纪律作风";
                db.Update<heart_message>(he, tran);
                heart_flow hf = new heart_flow();
                hf.Create();
                hf.messageid = he.id;
                hf.code = SystemInfo.Code;
                hf.name = SystemInfo.Name;
                db.Insert<heart_flow>(hf, tran);
                db.Commit();
                DDHelper.SendMSTDMessage(HeShujiInfo.Code, "民生通道审批提醒", $"{ hf.name }提交的民声通道纪律作风问题，请您批阅", SystemUrl);
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("留言异常：" + ex.Message);
                return ex.Message;
            }
        }

        [HttpDelete("{id}")]
        [EnableCors("any")]
        public string Delete(string id)
        {
            var tran = db.BeginTransaction();
            try
            {
                heart_message hesave = db.GetEntityById<heart_message>(id);
                DBHelper ssdb = new DBHelper("MSSQLCon");
                var dept = ssdb.GetEntity<DeptInfo>($"select top 1 Name,UserCode,(select ABBREVIATION+'-'+deptName from msgcenter_dept b where a.DeptCode=b.DeptCode) Dept,DingTalk from msgcenter_person a where UserCode = '{hesave.currentcode}'");
                hesave.state = -1;
                hesave.currentcode = "";
                hesave.nextcode = "";
                db.Update<heart_message>(hesave, tran);
                heart_flow hf = new heart_flow();
                hf.Create();
                hf.messageid = id;
                hf.code = dept.UserCode;
                hf.name = dept.Name;
                hf.refuse = "不回复";
                db.Insert<heart_flow>(hf, tran);
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("不回复操作异常：" + ex.Message);
                return ex.Message;
            }
        }

    }
}
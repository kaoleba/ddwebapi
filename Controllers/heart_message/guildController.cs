using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class guildController : ControllerBase
    {
        private DBHelper db = new DBHelper();

        private static UserInfo SystemInfo = new UserInfo("3708036537829904", "陈桂磊");//负责人
        private static string SystemUrl = "http://221.2.76.14:8055/#/dd_guild/syslist";//工会帮办办理Url
        private static string ReplyUrl = "http://221.2.76.14:8055/#/guild_reply";//工会帮办回复Url
        private static string UserUrl = "http://221.2.76.14:8055/#/guild_userlist";

        // GET api/controller
        [HttpGet]
        [EnableCors("any")]
        public ActionResult<IEnumerable<heart_guild>> Get(int? page, string state, string userid,string key)
        {
            try
            {
                if (page is null)
                    return null;
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select * from heart_guild  where 1=1 ");

                if (!string.IsNullOrEmpty(state))
                {
                    sql.Append(" and state=?state");
                    pars.Add("state", state);
                }
                if (!string.IsNullOrEmpty(userid))
                {
                    sql.Append(" and dingid = ?dingid ");
                    pars.Add("dingid", userid);
                }
                if (!string.IsNullOrEmpty(key))
                {
                    sql.Append(" and title like ?key or content like ?key");
                    pars.Add("key", "%" + key + "%");
                }
                return db.GetPageList<heart_guild>(sql.ToString(), pars, "createdate", "desc", Convert.ToInt32(page), 20);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取帮办信息异常：" + ex.Message);
                return null;
            }
        }


        // GET  api/proposal/5
        [HttpGet("{id}")]
        [EnableCors("any")]
        public ActionResult<heart_guild> Get(string id)
        {
            var pro = db.GetEntityById<heart_guild>(id);
            return pro;
        }

        /// <summary>
        /// 保存留言信息
        /// </summary>
        /// <param name="he"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")]
        [Route("SaveGuild")]
        public string SaveGuild(IFormFile file)
        {

            try
            {

                var imageList = "";
                IFormFileCollection cols = Request.Form.Files;
                foreach (IFormFile newfile in cols)
                {
                    string currentPictureExtension = Path.GetExtension(newfile.FileName).ToUpper();
                    var fileName = Guid.NewGuid() + currentPictureExtension;
                    var fileDir = "/wwwroot/images/" + fileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory() + fileDir);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        newfile.CopyTo(stream);
                        imageList += fileName + ",";
                    }
                }
                heart_guild hg = JsonHelper.ToJson<heart_guild>(Request.Form["guild"].ToString());
                hg.image = imageList;
                hg.Create();

                channelController cc = new channelController();
                DeptInfo dept =  cc.GetDeptInfo(hg.dingid);
                hg.code = dept.UserCode;
                hg.dept = dept.Dept;
                hg.mobile = dept.Phone;
                db.Insert<heart_guild>(hg);
                //给管理员发送通知消息
                DDHelper.SendGHBBMessage(SystemInfo.Code, "工会帮办审批提醒", "您有新的工会帮办需要处理", SystemUrl);
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("工会帮办新增异常：" + ex.Message);
                return ex.Message;
            }
        }


        [HttpGet]
        [EnableCors("any")]
        [Route("GetMessage")]
        public ActionResult<IEnumerable<heart_guild>> GetMessage()
        {
            try
            {
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select * from heart_guild  where state = 0 ");

                sql.Append(" order by createdate desc");
                return db.GetList<heart_guild>(sql.ToString(), pars);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取工会帮办异常：" + ex.Message);
                return null;
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
        public string SendReply(heart_guild he)
        {
            try
            {
                heart_guild hesave = db.GetEntityById<heart_guild>(he.id);

                hesave.state = 1;
                hesave.cldate =DateTime.Now;
                hesave.cldept = he.cldept;
                hesave.cldingid = he.cldingid;
                hesave.clusername = he.clusername;
                hesave.zpdingid = he.zpdingid;
                hesave.zpusername = he.zpusername;

                //给处理者发送待办信息            
                var res = DDHelper.SendReplyMessage(hesave.zpdingid, "工会帮办处理", "您有新的工会帮办消息需要处理", ReplyUrl + "?id=" + he.id);

                if (res.errorMsg == "")
                {
                    hesave.replyid = res.content.ToString();
                    db.Update<heart_guild>(hesave);
                    DDHelper.SendGHBBMessage(hesave.zpdingid, "工会帮办处理", "您有新的工会帮办消息需要处理", ReplyUrl + "?id=" + he.id);
                }
                else
                {
                    throw new Exception("推送待办异常：" + res.errorMsg);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("推送待办异常：" + ex.Message);
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
        public string SaveReply(heart_guild he)
        {
            try
            {
                heart_guild hesave = db.GetEntityById<heart_guild>(he.id);
                hesave.solve = he.solve;   
                hesave.solvedate = DateTime.Now;
                hesave.state = 2;
                //给处理者发送待办信息            
                var res = DDHelper.UpdateReplyMessage(hesave.zpdingid, hesave.replyid);

                if (res.errorMsg == "")
                {
                    db.Update<heart_guild>(hesave);
                    DDHelper.SendGHBBMessage(hesave.cldingid, "工会帮办处理", $"您指派的工会帮办已处理", SystemUrl);
                    DDHelper.SendGHBBMessage(hesave.dingid, "工会帮办处理", $"您提交的工会帮办已落实", UserUrl);
                }
                else
                {
                    throw new Exception("更新待办异常：" + res.errorMsg);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("保存处理信息异常：" + ex.Message);
                return ex.Message;
            }
        }


    }
}
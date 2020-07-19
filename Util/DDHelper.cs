using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DDWebApi
{
    public static class DDHelper
    {

        private static string CorpID = "ding50a1c2db19b1feb035c2f4657eb6378f";
        //string CorpSecret = "GnSdK_Gf74oXRXNj8QrVgb1jsDkMdT0WRdlEYXum-1a8JpabhwttkLcNVqkWDzG5";
        private static string CorpSecret = "_Is-qADPZH6Z6tQCng1dl-uG1wPZFo1VqJFsL22fcJYv2-wqA3NNZrq7532jNw68";
        private static string AgentID = "350136488";
        private static string Appkey = "dingbap32ep8awcpem4l";
        private static string Appsecret = "8Z3lI8FiM0AVi5suKlJ2fJ6qM6eAepx8UOrivuOqkEHT4nQ5x-lOG4qtYUumWu8U";

        /// <summary>
        /// 获取JSAPI鉴权（获取用户信息用不到）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ResponseResult GetConfig(string url)
        {
            ResponseResult rr = new ResponseResult();
            try
            {
                string nonceStr = Guid.NewGuid().ToString();

                long timeStamp = DDCommon.GetTimeStamp();
                string token = DDCommon.GetAccessToken(Appkey, Appsecret);
                string ticket = DDCommon.GetJsApiTicket(token);
                string signature = DDCommon.GetSign(ticket, nonceStr, timeStamp, url);
                DDconfig ddconfig = new DDconfig();
                ddconfig.JsApiTicket = ticket;
                ddconfig.Signature = signature;
                ddconfig.NonceStr = nonceStr;
                ddconfig.TimeStamp = timeStamp;
                ddconfig.CorpId = CorpID;
                ddconfig.CorpSecret = CorpSecret;
                ddconfig.AgentId = AgentID;
                ddconfig.access_token = token;
                ddconfig.Url = url;
                rr.content = ddconfig;
            }
            catch (Exception ex)
            {
                rr.errorMsg = ex.Message;
            }
            return rr;
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <returns></returns>
        public static ResponseResult GetUserInfo(string code, string token = "")
        {
            ResponseResult res = new ResponseResult();
            try
            {
                if (token == "")
                    token = DDCommon.GetAccessToken(Appkey, Appsecret);
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/getuserinfo");

                OapiUserGetuserinfoRequest request = new OapiUserGetuserinfoRequest();
                request.Code = code;
                request.SetHttpMethod("GET");
                OapiUserGetuserinfoResponse response = client.Execute<OapiUserGetuserinfoResponse>(request, token);
                String userId = response.Userid;
                if (response.Errmsg != "ok")
                {
                    res.errorMsg = response.Errmsg;
                }
                else
                {
                    res.content = response.Body.ToJson();
                }
                if (!string.IsNullOrEmpty(userId))
                {

                    IDingTalkClient clientUser = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/get");
                    OapiUserGetRequest requestUser = new OapiUserGetRequest();
                    requestUser.Userid = userId;
                    requestUser.SetHttpMethod("GET");


                    OapiUserGetResponse responseUser = clientUser.Execute<OapiUserGetResponse>(requestUser, token);
                    if (responseUser.Errmsg != "ok")
                    {
                        res.errorMsg = responseUser.Errmsg;
                    }
                    else
                    {
                        var dept = GetDeptName(responseUser.Department[0].ToString(), token);
                        JObject js = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(responseUser.Body);
                        if (dept.errorMsg == "")
                        {
                            js["remark"] = dept.content.ToString();
                        }
                        else
                        {
                            LogHelper.Error("获取部门信息异常：" + dept.errorMsg);
                        }
                        res.content = js.ToJson();
                    }
                }
            }
            catch (Exception ex)
            {
                res.errorMsg = ex.Message;
            }

            return res;
        }

        /// <summary>
        /// 获取钉钉部门信息
        /// </summary>
        /// <returns></returns>
        public static ResponseResult GetDeptName(string Id, string token = "")
        {
            ResponseResult res = new ResponseResult();
            try
            {
                if (token == "")
                    token = DDCommon.GetAccessToken(Appkey, Appsecret);
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/get");
                OapiDepartmentGetRequest req = new OapiDepartmentGetRequest();
                req.SetHttpMethod("GET");
                req.Id = Id;
                OapiDepartmentGetResponse rsp = client.Execute(req, token);
                if (rsp.Errmsg != "ok")
                {
                    res.errorMsg = rsp.ErrMsg;
                }
                else
                {
                    res.content = rsp.Name;
                }
            }
            catch (Exception ex)
            {
                res.errorMsg = ex.Message;
            }

            return res;
        }

        /// <summary>
        /// 发送通知信息
        /// </summary>
        /// <param name="code">员工编号</param>
        /// <param name="Title">标题</param>
        /// <param name="Text">文本</param>
        /// <param name="url">连接</param>
        /// <returns></returns>
        public static ResponseResult SendMSTDMessage(string code,string Title, string Text,string url)
        {
            DBHelper db = new DBHelper();
            ResponseResult res = new ResponseResult();
            try
            {
                DBHelper ssdb = new DBHelper("MSSQLCon");
                string  token = token = DDCommon.GetAccessToken("dingew639q0gbtjsspqa", "jLpYavso9CJftwBIFzgs8B1BprBJYXwVEVd8-tStvmrJujJE-QiV68azc8EOPn0T");
                List<string> list = ssdb.GetList<string>("select top 1  DingTalk from msgcenter_person where UserCode='" + code + "'");
                if (list.Count == 0)
                {
                    throw new Exception($"未找到{code}员工信息");
                }
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2");
                OapiMessageCorpconversationAsyncsendV2Request req = new OapiMessageCorpconversationAsyncsendV2Request();
                req.AgentId = 821139979L;
                req.UseridList = list[0];
                //req.UseridList = "05273505221222370";
                req.ToAllUser = false;
                OapiMessageCorpconversationAsyncsendV2Request.MsgDomain obj1 = new OapiMessageCorpconversationAsyncsendV2Request.MsgDomain();
                obj1.Msgtype = "action_card";

                OapiMessageCorpconversationAsyncsendV2Request.ActionCardDomain obj2 = new OapiMessageCorpconversationAsyncsendV2Request.ActionCardDomain();
                List<OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain> list4 = new List<OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain>();
                OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain obj5 = new OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain();
                list4.Add(obj5);
                obj5.ActionUrl = url;
                obj5.Title = "前往处理";
                obj2.Title = Title;
                obj2.BtnJsonList = list4;
                obj2.BtnOrientation = "0";
                obj2.Markdown = "![](http://221.2.76.14:8055/img/ms.jpg)" + "\n\n ### " + Text + "\n\n ### 时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                obj2.Title = Title;
                obj1.ActionCard = obj2;
                req.Msg_ = obj1;
                DingTalk.Api.Response.OapiMessageCorpconversationAsyncsendV2Response response = client.Execute<DingTalk.Api.Response.OapiMessageCorpconversationAsyncsendV2Response>(req, token);
                if (!string.IsNullOrEmpty(response.ErrMsg))
                {
                    res.errorMsg = response.ErrMsg;
                }
                if (!string.IsNullOrEmpty(response.Errmsg))
                {
                    res.errorMsg = response.Errmsg;
                }
            }
            catch (Exception ex)
            {
                res.errorMsg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 发送通知信息
        /// </summary>
        /// <param name="code">员工编号</param>
        /// <param name="Title">标题</param>
        /// <param name="Text">文本</param>
        /// <param name="url">连接</param>
        /// <returns></returns>
        public static ResponseResult SendGHBBMessage(string code, string Title, string Text, string url)
        {
            DBHelper db = new DBHelper();
            ResponseResult res = new ResponseResult();
            try
            {

                string token = token = DDCommon.GetAccessToken("dingd0n4dknpwbjikxm4", "o7Ahj9AOl4GLzmTL5fmOWpVxtH8KlftbVGupwp8SgNeosUWEADOEhS69S1C3MiGW");

                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2");
                OapiMessageCorpconversationAsyncsendV2Request req = new OapiMessageCorpconversationAsyncsendV2Request();
                req.AgentId = 821201450L;
                req.UseridList = code;
                //req.UseridList = "05273505221222370";
                req.ToAllUser = false;
                OapiMessageCorpconversationAsyncsendV2Request.MsgDomain obj1 = new OapiMessageCorpconversationAsyncsendV2Request.MsgDomain();
                obj1.Msgtype = "action_card";

                OapiMessageCorpconversationAsyncsendV2Request.ActionCardDomain obj2 = new OapiMessageCorpconversationAsyncsendV2Request.ActionCardDomain();
                List<OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain> list4 = new List<OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain>();
                OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain obj5 = new OapiMessageCorpconversationAsyncsendV2Request.BtnJsonListDomain();
                list4.Add(obj5);
                obj5.ActionUrl = url;
                obj5.Title = "前往处理";
                obj2.Title = Title;
                obj2.BtnJsonList = list4;
                obj2.BtnOrientation = "0";
                obj2.Markdown = "![](http://221.2.76.14:8055/img/gh.jpg)" + "\n\n ### " + Text + "\n\n ### 时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                obj2.Title = Title;
                obj1.ActionCard = obj2;
                req.Msg_ = obj1;
                DingTalk.Api.Response.OapiMessageCorpconversationAsyncsendV2Response response = client.Execute<DingTalk.Api.Response.OapiMessageCorpconversationAsyncsendV2Response>(req, token);
                if (!string.IsNullOrEmpty(response.ErrMsg))
                {
                    res.errorMsg = response.ErrMsg;
                }
                if (!string.IsNullOrEmpty(response.Errmsg))
                {
                    res.errorMsg = response.Errmsg;
                }
            }
            catch (Exception ex)
            {
                res.errorMsg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 发送待办信息
        /// </summary>
        /// <param name="dingid">钉钉ID</param>
        /// <param name="Title">标题</param>
        /// <param name="Text">文本</param>
        /// <param name="url">url连接</param>
        /// <returns></returns>
        public static ResponseResult SendReplyMessage(string dingid, string Title, string Text, string url)
        {
            DBHelper db = new DBHelper();
            ResponseResult res = new ResponseResult();
            try
            {
     
                string token = token = DDCommon.GetAccessToken("dingew639q0gbtjsspqa", "jLpYavso9CJftwBIFzgs8B1BprBJYXwVEVd8-tStvmrJujJE-QiV68azc8EOPn0T");
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/workrecord/add");
                OapiWorkrecordAddRequest req = new OapiWorkrecordAddRequest();
                req.Userid = dingid;
                req.CreateTime = DDCommon.GetTimeStamp();
                req.Title = Title;
                req.Url = url;
                req.PcUrl = url;
                List<OapiWorkrecordAddRequest.FormItemVoDomain> list2 = new List<OapiWorkrecordAddRequest.FormItemVoDomain>();
                OapiWorkrecordAddRequest.FormItemVoDomain obj3 = new OapiWorkrecordAddRequest.FormItemVoDomain();
                list2.Add(obj3);
                obj3.Title = Title;
                obj3.Content = Text;
                req.FormItemList_ = list2;
                req.PcOpenType = 2L;
                OapiWorkrecordAddResponse response = client.Execute(req, token);
                if (!string.IsNullOrEmpty(response.ErrMsg))
                {
                    res.errorMsg = response.ErrMsg;
                }
                if (!string.IsNullOrEmpty(response.Errmsg))
                {
                    res.errorMsg = response.Errmsg;
                }
                res.content = response.RecordId;
            }
            catch (Exception ex)
            {
                res.errorMsg = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 更新待办状态
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        public static ResponseResult UpdateReplyMessage(string Userid, string RecordId)
        {
            DBHelper db = new DBHelper();
            ResponseResult res = new ResponseResult();
            try
            {

                string token = token = DDCommon.GetAccessToken("dingew639q0gbtjsspqa", "jLpYavso9CJftwBIFzgs8B1BprBJYXwVEVd8-tStvmrJujJE-QiV68azc8EOPn0T");
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/workrecord/update");
                OapiWorkrecordUpdateRequest req = new OapiWorkrecordUpdateRequest();
                req.Userid = Userid;
                req.RecordId = RecordId;
                OapiWorkrecordUpdateResponse response = client.Execute(req, token);
                if (!string.IsNullOrEmpty(response.ErrMsg))
                {
                    res.errorMsg = response.ErrMsg;
                }
                if (!string.IsNullOrEmpty(response.Errmsg))
                {
                    res.errorMsg = response.Errmsg;
                }
            }
            catch (Exception ex)
            {
                res.errorMsg = ex.Message;
            }
            return res;
        }

    }
}
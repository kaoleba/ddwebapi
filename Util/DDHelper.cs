using Dapper;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

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
        /// 获取JSAPI鉴权
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
        /// 获取用户详情
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
                    res.errorMsg = rsp.Errmsg;
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

    }
}
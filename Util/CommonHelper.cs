using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DDWebApi
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp(this HttpContext context)
        {

            string ip = "未得到Ip地址";
            try
            {
                ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            catch 
            { }
            return ip;
        }
    }

    public static class AutoMaperHelper
    {
        public static TDestination MapTo<TDestination, TSource>(this TSource source, TDestination des)
        where TDestination : class
        where TSource : class
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource,TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map(source, des);
        }
    }

    public class HttpRequestHelper
    {
        public static string Get(string url)
        {
            WebRequest request = HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            return content;
        }

        public static string Post(string url)
        {
            WebRequest request = HttpWebRequest.Create(url);
            request.Method = "POST";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            return content;
        }
    }


    public class DDCommon
    {

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string corpId, string corpSecret)
        {
            string url = string.Format("https://oapi.dingtalk.com/gettoken?appkey={0}&appsecret={1}", corpId, corpSecret);
            try
            {
                string response = HttpRequestHelper.Get(url);
                AccessTokenModel oat = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessTokenModel>(response);
                if (oat != null)
                {
                    if (oat.errcode == 0)
                    {
                        return oat.access_token;
                    }
                }
            }
            catch
            {
                throw;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取ApiTicket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetJsApiTicket(string accessToken)
        {
            string url = string.Format("https://oapi.dingtalk.com/get_jsapi_ticket?access_token={0}", accessToken);
            try
            {
                string response = HttpRequestHelper.Get(url);
                JsApiTicketModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<JsApiTicketModel>(response);
                if (model != null)
                {
                    if (model.errcode == 0)
                    {
                        return model.ticket;
                    }
                    else
                    {
                        throw new Exception(response);
                    }
                }
            }
            catch
            {
                throw;
            }
            return string.Empty;
        }

        public static long GetTimeStamp(DateTime time)
        {

            return (long)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static long GetTimeStamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static string GetSign(string ticket, string nonceStr, long timeStamp, string url)
        {
            String plain = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, nonceStr, timeStamp.ToString(), url);
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plain);
                byte[] digest = SHA1.Create().ComputeHash(bytes);
                string digestBytesString = BitConverter.ToString(digest).Replace("-", "");
                return digestBytesString.ToLower();
            }
            catch
            {
                throw;
            }
        }
    }
}
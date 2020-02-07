using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWebApi
{
    public class DDconfig
    {
        public string CorpId { get; set; }
        public string CorpSecret { get; set; }
        public string AgentId { get; set; }
        public long TimeStamp { get; set; }
        public string Signature { get; set; }
        public string JsApiTicket { get; set; }
        public string NonceStr { get; set; }
        public string access_token { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// Token类
    /// </summary>
    public class AccessTokenModel
    {
        public string access_token { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }

    /// <summary>
    /// Ticket类
    /// </summary>
    public class JsApiTicketModel
    {
        public string access_token { get; set; }
        public int errcode { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDWebApi.Models
{

    public class NewsItem
    {
        public string name { get; set; }
        public string bm { get; set; }
        public string wbtitle { get; set; }
        public int wbreply { get; set; }
        public string wbdate { get; set; }
        public int wbmessageid { get; set; }
        public string wbrecontent { get; set; }
    }

    public class NewsList
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NewsItem> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
    }


    public class NewsDetail
    {
        public string wbip { get; set; }
        public string wbcontent { get; set; }
        public string wbimage { get; set; }
        public string wbattach { get; set; }
        public int wbaudit { get; set; }
        public int wbreply { get; set; }
        public string wbdate { get; set; }
        public string wbredate { get; set; }
        public string bm { get; set; }
        public string name { get; set; }
        public string wbrevertimage { get; set; }
        public string wbtitle { get; set; }
        public string wbreaccount { get; set; }
        public int wbmessageid { get; set; }
        public string wbrecontent { get; set; }
    }

    public class NewsDetailList
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NewsDetail> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
    }


    public class AddItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string wbip { get; set; }
        /// <summary>
        /// 测试一下留言内容
        /// </summary>
        public string wbcontent { get; set; }
        /// <summary>
        /// 陈琪测试
        /// </summary>
        public string wbtitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int wbmessageid { get; set; }
    }

    public class AddResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<AddItem> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
    }


    public class ReplyResult
    {
        /// <summary>
        /// 
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
    }
}

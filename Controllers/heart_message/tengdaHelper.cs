using DDWebApi.Models;
using System;
using System.Text;

namespace DDWebApi.Controllers
{
    public class tengdaHelper
    {
        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <param name="hm"></param>
        /// <returns></returns>
        public static string NewMessange(heart_message hm)
        {
            try
            {
                string url = $"http://10.0.11.33:8080/system/resource/messageApi/addMessage2.jsp?wbviewid=1841&owner=886666253&wbtitle={hm.title}&wbuserid=0&wbcontent={hm.message}&wbip={hm.ip}&wbmessagetypeid=0";
                var res = HttpRequestHelper.Post(url).ToJson<AddResult>();
                return res.data[0].wbmessageid.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("调用山能新增接口异常：" + ex.Message);
            }
        }

        public static void ReplyMessage(heart_message hm)
        {
            try
            {
                string url = $"http://10.0.11.33:8080/system/resource/messageApi/messageRelpy.jsp?wbviewid=1841&owner=886666253&wbmessageid={hm.id}&wbcontent={hm.message}&wbreaccount=党办管理员&wbredate={Convert.ToDateTime(hm.publicdate).ToString("yyyy-MM-dd HH:mm:ss")}&wbrecontent={hm.reply}";
                var res = HttpRequestHelper.Post(url).ToJson<ReplyResult>();
                if(res.code!=200)
                {
                    throw new Exception(res.data.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用山能新增接口异常：" + ex.Message);
            }
        }


        public static void GetNewsList()
        {

            try
            {
                DBHelper db = new DBHelper();
                string url = "http://10.0.11.33:8080/system/resource/messageApi/queryMessageList.jsp?wbviewid=1841&owner=886666253&nowPage=1&numPerPage=1000";
                var res = HttpRequestHelper.Post(url).ToJson<NewsList>();



                foreach (var item in res.data)
                {
                    heart_message hr = new heart_message();
                    if (!string.IsNullOrEmpty(item.wbrecontent))
                    {
                        item.wbrecontent = ReplaceHtmlTag(item.wbrecontent);
                        hr.state = 2;
                        hr.reply = item.wbrecontent.Trim();
                    }
                    else
                    {
                        if (DateTime.Now.AddDays(-3) >= Convert.ToDateTime(item.wbdate))
                        {
                            hr.state = -1;
                        }
                        else
                        {
                            hr.state = 0;
                        }
                    }
                    string detailUrl = $"http://10.0.11.33:8080/system/resource/messageApi/queryMessageDetails.jsp?wbviewid=1841&owner=886666253&wbmessageid={item.wbmessageid}";
                    var detail = HttpRequestHelper.Post(detailUrl).ToJson<NewsDetailList>();
                    var message = detail.data[0];
                    message.wbcontent = ReplaceHtmlTag(message.wbcontent);
                    hr.message = message.wbcontent.Trim();
                    hr.createdate = Convert.ToDateTime(item.wbdate);
                    hr.id = item.wbmessageid.ToString();
                    hr.ip = message.wbip;
                    hr.title = item.wbtitle;
                    hr.origin = "TB";
                    hr.currentcode = "10034488";
                    db.Insert<heart_message>(hr);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("请求接口未成功");
            }
        }

        public static string ReplaceHtmlTag(string html)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            return filterEmoji(strText);
        }

        public static string filterEmoji(string str)
        {

            string origin = str;
            try
            {
                //关键代码
                foreach (var a in str)
                {
                    byte[] bts = Encoding.UTF32.GetBytes(a.ToString());
                    if (bts[0].ToString() == "253" && bts[1].ToString() == "255")
                    {
                        str = str.Replace(a.ToString(), "");
                    }
                }
            }
            catch (Exception)
            {
                str = origin;
            }
            return str;
        }
    }
}

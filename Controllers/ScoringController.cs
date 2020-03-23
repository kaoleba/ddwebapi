using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDWebApi.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class ScoringController : ControllerBase
    {
        private DBHelper db = new DBHelper();

        // GET api/scoring
        [HttpGet]
        [EnableCors("any")]
        public ActionResult<Griddto> Get()
        {
            try
            {
                Griddto result = new Griddto();
                List<scorelist> scolist = new List<scorelist>();

                var list = db.GetList<ScoringDetailList>("select a.type,b.* from scoring a left join scoring_detail b on a.id=b.pid where a.title = '纪委书记履职情况评议表'");
                result.jtjw = db.QueryFirstOrDefault<int>("select count(1) from  scoring  where type='集团公司纪委人员'");
                result.qtry = db.QueryFirstOrDefault<int>("select count(1) from  scoring  where type='各单位其他人员'");
                result.dwjw = db.QueryFirstOrDefault<int>("select count(1) from  scoring  where type='各单位纪委书记'");
                result.total = result.jtjw + result.qtry + result.dwjw;
                string[] listName = { "董忠科", "秦 涛","张 卫","杨现贵","魏海峰","付士军","董凤广","孙迎东","张世海","吴同德","白云明","翟若臣","赵钦营","徐晓华", "胡彦峰" };
               
                for (int i = 0; i < listName.Length; i++)
                {
                    scorelist scoreuser = new scorelist();
                    string username = listName[i];
                    scoreuser.name = username;
                    scoreuser.zt1 = list.Where(item => item.UserName == username && item.ScoreTotal == "4").Count().ToString();
                    scoreuser.zt2 = list.Where(item => item.UserName == username && item.ScoreTotal == "3").Count().ToString();
                    scoreuser.zt3 = list.Where(item => item.UserName == username && item.ScoreTotal == "2").Count().ToString();
                    scoreuser.zt4 = list.Where(item => item.UserName == username && item.ScoreTotal == "1").Count().ToString();

                    scoreuser.ls1 = list.Where(item => item.UserName == username && item.Score1 == "4").Count().ToString();
                    scoreuser.ls2 = list.Where(item => item.UserName == username && item.Score1 == "3").Count().ToString();
                    scoreuser.ls3 = list.Where(item => item.UserName == username && item.Score1 == "2").Count().ToString();
                    scoreuser.ls4 = list.Where(item => item.UserName == username && item.Score1 == "1").Count().ToString();

                    scoreuser.jj1 = list.Where(item => item.UserName == username && item.Score2 == "4").Count().ToString();
                    scoreuser.jj2 = list.Where(item => item.UserName == username && item.Score2 == "3").Count().ToString();
                    scoreuser.jj3 = list.Where(item => item.UserName == username && item.Score2 == "2").Count().ToString();
                    scoreuser.jj4 = list.Where(item => item.UserName == username && item.Score2 == "1").Count().ToString();

                    scoreuser.zf1 = list.Where(item => item.UserName == username && item.Score3 == "4").Count().ToString();
                    scoreuser.zf2 = list.Where(item => item.UserName == username && item.Score3 == "3").Count().ToString();
                    scoreuser.zf3 = list.Where(item => item.UserName == username && item.Score3 == "2").Count().ToString();
                    scoreuser.zf4 = list.Where(item => item.UserName == username && item.Score3 == "1").Count().ToString();

                    scoreuser.lssj1 = list.Where(item => item.UserName == username && item.Score4 == "4").Count().ToString();
                    scoreuser.lssj2 = list.Where(item => item.UserName == username && item.Score4 == "3").Count().ToString();
                    scoreuser.lssj3 = list.Where(item => item.UserName == username && item.Score4 == "2").Count().ToString();
                    scoreuser.lssj4 = list.Where(item => item.UserName == username && item.Score4 == "1").Count().ToString();

                    scoreuser.jjzz1 = list.Where(item => item.UserName == username && item.Score5 == "4").Count().ToString();
                    scoreuser.jjzz2 = list.Where(item => item.UserName == username && item.Score5 == "3").Count().ToString();
                    scoreuser.jjzz3 = list.Where(item => item.UserName == username && item.Score5 == "2").Count().ToString();
                    scoreuser.jjzz4 = list.Where(item => item.UserName == username && item.Score5 == "1").Count().ToString();

                    scoreuser.wtzz1 = list.Where(item => item.UserName == username && item.Score6 == "4").Count().ToString();
                    scoreuser.wtzz2 = list.Where(item => item.UserName == username && item.Score6 == "3").Count().ToString();
                    scoreuser.wtzz3 = list.Where(item => item.UserName == username && item.Score6 == "2").Count().ToString();
                    scoreuser.wtzz4 = list.Where(item => item.UserName == username && item.Score6 == "1").Count().ToString();

                    scoreuser.gzzz1 = list.Where(item => item.UserName == username && item.Score7 == "4").Count().ToString();
                    scoreuser.gzzz2 = list.Where(item => item.UserName == username && item.Score7 == "3").Count().ToString();
                    scoreuser.gzzz3 = list.Where(item => item.UserName == username && item.Score7 == "2").Count().ToString();
                    scoreuser.gzzz4 = list.Where(item => item.UserName == username && item.Score7 == "1").Count().ToString();

                    scolist.Add(scoreuser);

                }

                result.list = scolist;

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取访问记录异常：" + ex.Message);
                return null;
            }                                               
        }


        public string GetRes(List<ScoringDetailList> list, string name, string field, string score, double totalA, double totalB, double totalC)
        {
            string spitChar = "\r\n";
            double A = list.Where(item => item.UserName == name && item.GetType().GetProperty(field).GetValue(item).ToString() == score && item.Type == "集团公司纪委人员").Count();
            double B = list.Where(item => item.UserName == name && item.GetType().GetProperty(field).GetValue(item).ToString() == score && item.Type == "各单位纪委书记").Count();
            double C = list.Where(item => item.UserName == name && item.GetType().GetProperty(field).GetValue(item).ToString() == score && item.Type == "各单位其他人员").Count();
            double manyi = 0;

            if (totalA > 0)
                manyi += ((A / totalA) * 0.5);
            if (totalB > 0)
                manyi += ((B / totalB) * 0.3);
            if (totalC > 0)
                manyi += ((C / totalC) * 0.2);
            manyi = Math.Round(manyi, 4)*100;

            string res = $"A:{A}{spitChar}B:{B}{spitChar}C:{C}{spitChar}{manyi}%";
            return res;
        }


        // GET api/scoring/GetManyi
        [HttpGet]
        [EnableCors("any")]
        [Route("GetManyi")]
        public ActionResult<Griddto> GetManyi()
        {
            try
            {
                Griddto result = new Griddto();
                List<scorelist> scolist = new List<scorelist>();
                var list = db.GetList<ScoringDetailList>("select a.type,b.* from scoring a left join scoring_detail b on a.id=b.pid where a.title = '纪委书记履职情况评议表'");
                double totalA = db.QueryFirstOrDefault<int>("select count(1) from  scoring  where type='集团公司纪委人员'");
                double totalB = db.QueryFirstOrDefault<int>("select count(1) from  scoring  where type='各单位纪委书记'");
                double totalC = db.QueryFirstOrDefault<int>("select count(1) from  scoring  where type='各单位其他人员'");
                string[] listName = { "董忠科", "秦 涛", "张 卫", "杨现贵", "魏海峰", "付士军", "董凤广", "孙迎东", "张世海", "吴同德", "白云明", "翟若臣", "赵钦营", "徐晓华", "胡彦峰" };
                for (int i = 0; i < listName.Length; i++)
                {
                    scorelist scoreuser = new scorelist();
                    string username = listName[i];
                    scoreuser.name = username;
                    scoreuser.zt1 = GetRes(list, username, "ScoreTotal", "4", totalA, totalB, totalC);
                    scoreuser.zt2 = GetRes(list, username, "ScoreTotal", "3", totalA, totalB, totalC);
                    scoreuser.zt3 = GetRes(list, username, "ScoreTotal", "2", totalA, totalB, totalC);
                    scoreuser.zt4 = GetRes(list, username, "ScoreTotal", "1", totalA, totalB, totalC);

                    scoreuser.ls1 = GetRes(list, username, "Score1", "4", totalA, totalB, totalC);
                    scoreuser.ls2 = GetRes(list, username, "Score1", "3", totalA, totalB, totalC);
                    scoreuser.ls3 = GetRes(list, username, "Score1", "2", totalA, totalB, totalC);
                    scoreuser.ls4 = GetRes(list, username, "Score1", "1", totalA, totalB, totalC);

                    scoreuser.jj1 = GetRes(list, username, "Score2", "4", totalA, totalB, totalC);
                    scoreuser.jj2 = GetRes(list, username, "Score2", "3", totalA, totalB, totalC);
                    scoreuser.jj3 = GetRes(list, username, "Score2", "2", totalA, totalB, totalC);
                    scoreuser.jj4 = GetRes(list, username, "Score2", "1", totalA, totalB, totalC);

                    scoreuser.zf1 = GetRes(list, username, "Score3", "4", totalA, totalB, totalC);
                    scoreuser.zf2 = GetRes(list, username, "Score3", "3", totalA, totalB, totalC);
                    scoreuser.zf3 = GetRes(list, username, "Score3", "2", totalA, totalB, totalC);
                    scoreuser.zf4 = GetRes(list, username, "Score3", "1", totalA, totalB, totalC);

                    scoreuser.lssj1 = GetRes(list, username, "Score4", "4", totalA, totalB, totalC);
                    scoreuser.lssj2 = GetRes(list, username, "Score4", "3", totalA, totalB, totalC);
                    scoreuser.lssj3 = GetRes(list, username, "Score4", "2", totalA, totalB, totalC);
                    scoreuser.lssj4 = GetRes(list, username, "Score4", "1", totalA, totalB, totalC);

                    scoreuser.jjzz1 = GetRes(list, username, "Score5", "4", totalA, totalB, totalC);
                    scoreuser.jjzz2 = GetRes(list, username, "Score5", "3", totalA, totalB, totalC);
                    scoreuser.jjzz3 = GetRes(list, username, "Score5", "2", totalA, totalB, totalC);
                    scoreuser.jjzz4 = GetRes(list, username, "Score5", "1", totalA, totalB, totalC);

                    scoreuser.wtzz1 = GetRes(list, username, "Score6", "4", totalA, totalB, totalC);
                    scoreuser.wtzz2 = GetRes(list, username, "Score6", "3", totalA, totalB, totalC);
                    scoreuser.wtzz3 = GetRes(list, username, "Score6", "2", totalA, totalB, totalC);
                    scoreuser.wtzz4 = GetRes(list, username, "Score6", "1", totalA, totalB, totalC);

                    scoreuser.gzzz1 = GetRes(list, username, "Score7", "4", totalA, totalB, totalC);
                    scoreuser.gzzz2 = GetRes(list, username, "Score7", "3", totalA, totalB, totalC);
                    scoreuser.gzzz3 = GetRes(list, username, "Score7", "2", totalA, totalB, totalC);
                    scoreuser.gzzz4 = GetRes(list, username, "Score7", "1", totalA, totalB, totalC);

                    scolist.Add(scoreuser);

                }

                result.list = scolist;

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取访问记录异常：" + ex.Message);
                return null;
            }
        }

        [HttpPost]
        [EnableCors("any")]
        [Route("SaveSore")]
        public string SaveSore(object score)
        {
            DBHelper db = new DBHelper();
            var tran = db.BeginTransaction();
            try
            {
                Newtonsoft.Json.Linq.JObject res = score as Newtonsoft.Json.Linq.JObject;

                Scoring scoremain = new Scoring();
                scoremain.Create();
                try
                {
                    scoremain.Ip = HttpContext.GetClientUserIp();
                    scoremain.Title = res["title"].ToString();
                    scoremain.Type = res["type"].ToString();
                }
                catch
                { }
                db.Insert<Scoring>(scoremain, tran);

                var length = res["score"].Count();
                for (int i = 0; i < length; i++)
                {
                    ScoringDetail detail = new ScoringDetail();
                    var item = res["score"].ElementAt(i);
                    var name = ((Newtonsoft.Json.Linq.JProperty)item).Name;
                    var values = ((Newtonsoft.Json.Linq.JProperty)item).Value;
                    detail.UserName = name;
                    detail.Create();
                    detail.PId = scoremain.Id;
                    detail.ScoreTotal = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(0).First).Value.ToString();
                    try
                    {
                        detail.Score1 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(1).First).Value.ToString();
                        detail.Score2 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(2).First).Value.ToString();
                        detail.Score3 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(3).First).Value.ToString();
                        detail.Score4 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(4).First).Value.ToString();
                        detail.Score5 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(5).First).Value.ToString();
                        detail.Score6 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(6).First).Value.ToString();
                        detail.Score7 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(7).First).Value.ToString();
                    }
                    catch
                    {
                    }
                    db.Insert<ScoringDetail>(detail, tran);
                }
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                LogHelper.Error("测评异常：" + ex.Message);
                return ex.Message;
            }
        }
    }
}
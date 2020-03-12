using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DDWebApi.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class ScoringController : ControllerBase
    {

    

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
                scoremain.Ip = HttpContext.GetClientUserIp();
                scoremain.Title = res["title"].ToString();
                scoremain.Type = res["type"].ToString() ;
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
                    detail.ScoreTotal=((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(0).First).Value.ToString();
                    detail.Score1 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(1).First).Value.ToString();
                    detail.Score2 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(2).First).Value.ToString();
                    detail.Score3 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(3).First).Value.ToString();
                    detail.Score4 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(4).First).Value.ToString();
                    detail.Score5 = ((Newtonsoft.Json.Linq.JValue)values.ElementAtOrDefault(5).First).Value.ToString();
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
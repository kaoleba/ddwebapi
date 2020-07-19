using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DDWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        public class FoodsEvaluate
        {
            public string questionid { get; set; }
            public string userid { get; set; }
            public List<foodDto> fooddto { get; set; }
            public List<object> values { get; set; }
            public DateTime enddateTime;
            public bool save;
            public string errorMsg { get; set; }
        }

        public class foodDto
        {
            public string Id { get; set; }
            public string name { get; set; }
            public string pic { get; set; }
            public int value1 { get; set; }
            public int value2 { get; set; }
        }

        private DBHelper db = new DBHelper();

        [HttpGet]
        [Route("GetFoods")]
        [EnableCors("any")]
        public FoodsEvaluate GetFoods(string questionid, string userid)
        {
            FoodsEvaluate feo = new FoodsEvaluate();
            feo.errorMsg = "";
            try
            {
                var qustion =  db.GetEntityById<qustion>(questionid);
                if (qustion.EndTime <= DateTime.Now)
                {
                    throw new Exception("本次食堂评价已结束！");
                }

                DynamicParameters pars = new DynamicParameters();
                pars.Add("questionid", questionid);
                pars.Add("userid", userid);
                var list = db.GetList<foodDto>("select a.Id,a.name,a.pic,b.value1,b.value2 from foods a left join foods_evaluate b on a.id=b.foodid  and b.userid=?userid where a.questionid=?questionid order by Sort", pars);
                foreach (var food in list)
                {
                    food.pic = food.pic.Replace("~", "http://221.2.76.14:8059");
                }

                var rem = db.QueryFirstOrDefault<foods_user>("select * from foods_user where questionid=?questionid and userid=?userid", pars);
                feo.values = new List<object>();
                if (rem != null)
                {
                    feo.fooddto = list;

                    feo.values.Add(rem.value1 == null ? 0 : rem.value1);
                    feo.values.Add(rem.value2 == null ? 0 : rem.value2);
                    feo.values.Add(rem.value3 == null ? 0 : rem.value3);
                    feo.values.Add(rem.remark1 == null ? "" : rem.remark1);
                    feo.values.Add(rem.remark2 == null ? "" : rem.remark2);
                    feo.save = false;
                }
                else
                {
                    feo.save = true;
                }

                if (Convert.ToInt32(feo.values[0]) != 0)
                {
                    feo.save = true;
                }
                

                //else
                //{
                //    throw new Exception("未找到您的参评人员信息！");     
                //}

                feo.questionid = questionid;
                feo.userid = userid;

            }
            catch (Exception ex)
            {
                feo.save = true;
                feo.errorMsg = ex.Message;
            }
            return feo;
        }

        [HttpPost]
        [Route("SaveFoods")]
        [EnableCors("any")]
        public string SaveFoods(FoodsEvaluate foodseva)
        {

            try
            {
                db.BeginTransaction();
                foreach (var fto in foodseva.fooddto)
                {
                    db.ExecuteSql($"delete from foods_evaluate where userid='{foodseva.userid}' and foodid='{fto.Id}'");
                    foods_evaluate fe = new foods_evaluate();
                    fe.Create();
                    fe.foodid = fto.Id;
                    fe.userid = foodseva.userid;
                    fe.value1 = fto.value1;
                    fe.value2 = fto.value2;
                    db.Insert<foods_evaluate>(fe);
                }

                foods_user fd = db.QueryFirstOrDefault<foods_user>($"select * from foods_user where questionid='{foodseva.questionid}' and userid='{foodseva.userid}'");
                fd.value1 = Convert.ToInt32(foodseva.values[0]);
                fd.value2 = Convert.ToInt32(foodseva.values[1]);
                fd.value3 = Convert.ToInt32(foodseva.values[2]);
                fd.remark1 = Convert.ToString(foodseva.values[3]);
                fd.remark2 = Convert.ToString(foodseva.values[4]);
                fd.SubmitDate = DateTime.Now;
                db.Update<foods_user>(fd);
                db.Commit();
                return "";
            }
            catch (Exception ex)
            {
                db.Rollback();
                return ex.Message;
            }
        }

    }
}

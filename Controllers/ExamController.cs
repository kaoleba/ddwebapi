using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DDWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        public class LK_RYRanking
        {
            public DateTime ExamDate { get; set; }
            public string UserName { get; set; }
            public string Code { get; set; }
            public int Second { get; set; }
            public DateTime ExamTime;
            public string Company;
            public string Dept { get; set; }
            public double Score { get; set; }
            public int DWRancking { get; set; }
            public int JTRancking { get; set; }
        }

        public class LK_DeptRanking
        {
            public string Company { get; set; }
            public int ZCNumber { get; set; }
            public int CSNumber { get; set; }
            public double CSBL { get; set; }
            public int CSPM;
            public int HJCJ;
            public double PJCJ { get; set; }
            public double ZCJ { get; set; }
            public int CJPM { get; set; }
            public int Rank { get; set; }
            public DateTime ExamDate { get; set; }
        }


        private DBHelper db = new DBHelper("MSSQLExamCon");

        [HttpGet]
        [Route("GetRank")]
        [EnableCors("any")]
        public LK_RYRanking GetRank(string Code,DateTime ExamDate)
        {
            DynamicParameters pars = new DynamicParameters();
            pars.Add("@ExamDate", ExamDate, System.Data.DbType.DateTime);
            pars.Add("@Code", Code);
            var rank = db.QueryFirstOrDefault<LK_RYRanking>("select * from LK_RYRanking where ExamDate=@ExamDate and Code=@Code", pars);
            return rank;
        }

        [HttpGet]
        [Route("GetDeptRank")]
        [EnableCors("any")]
        public List<LK_DeptRanking> GetDeptRank(DateTime ExamDate)
        {
            DynamicParameters pars = new DynamicParameters();
            pars.Add("@ExamDate", ExamDate, System.Data.DbType.DateTime);
            var rank = db.GetList<LK_DeptRanking>("select * from LK_DeptRanking where ExamDate=@ExamDate order by Rank", pars);
            return rank;
        }


    }
}

using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        DBHelper db = new DBHelper();
        [HttpGet]
        [EnableCors("any")]
        [Route("VoteInTimeCount")]
        public string VoteInTimeCount(string dept) {
            string sql = "select vote_id from vote where vote_dept='"+dept+"' and now() BETWEEN start_time and end_time limit 0,1";
            return db.GetEntity<string>(sql);
        }

        [HttpPost]
        [EnableCors("any")]
        [Route("SaveVoteInfo")]
        public int SaveVoteInfo(VoteDetail voteDetail) {
            voteDetail.vote_detail_id = System.Guid.NewGuid().ToString();
            voteDetail.vote_detail_def2 = DateTime.Now.ToString();
            db.Insert<VoteDetail>(voteDetail);
            return 1;
        }

        [HttpGet]
        [EnableCors("any")]
        [Route("CheckSubmit")]
        public int CheckSubmit(string userid,string type) {
            string sql = "select count(*) from vote_detail where vote_detail_def1='"+userid+"' and vote_detail_def3='"+type+"'";
            return db.GetEntity<int>(sql);
        }

        [HttpGet]
        [EnableCors("any")]
        [Route("report")]
        public List<report> Report(string type)
        {
            string sql = @"select vote_dept,vote_detail_name,count(*) count from vote_detail a left join vote b on a.vote_id=b.vote_idwhere vote_detail_def3='"+type+@"'
group by vote_dept,vote_detail_name order by count desc";
            return db.GetList<report>(sql);
        }

    }
}

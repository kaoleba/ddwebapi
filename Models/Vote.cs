using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWebApi
{
    [Table("vote_detail")]
    //投票活动
    public class Vote
    {
        public string vote_id { get; set; }
        public string vote_title { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string vote_dept { get; set; }
    }

    //投票详情
    [Table("vote_detail")]
    public class VoteDetail {
        public string vote_detail_id { get; set; }
        public string vote_id { get; set; }
        public string vote_detail_name { get; set; }
        public string vote_detail_duty { get; set; }
        public string vote_detail_info { get; set; }
        public int illegal_flag { get; set; }
        public string illegal_info { get; set; }
        public string vote_detail_def1 { get; set; }
        public string vote_detail_def2 { get; set; }
        public string vote_detail_def3 { get; set; }
    }

    public class report {
        public string vote_dept { get; set; }
        public string vote_detail_name { get; set; }
        public int count { get; set; }
        public string illegal_flag { get; set; }
    }


}

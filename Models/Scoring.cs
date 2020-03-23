using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDWebApi
{
    [Table("scoring")]
    /// <summary>
    /// 评分表
    /// </summary>
    public class Scoring
    {
        [ExplicitKey]
        public System.String Id { get; set; }

        /// <summary>
        /// 表头
        /// </summary>
        public System.String Title { get; set; }

        /// <summary>
        /// 评测人类型
        /// </summary>
        public System.String Type { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public System.String Ip { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime? CreateTime { get; set; }

        public void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }
    }

    [Table("scoring_detail")]
    /// <summary>
    /// 评分表
    /// </summary>
    public class ScoringDetail
    {
        [ExplicitKey]
        public System.String Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public System.String PId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public System.String UserName { get; set; }

        /// <summary>
        /// 评测总分
        /// </summary>
        public System.String ScoreTotal { get; set; }

        /// <summary>
        /// 分数1
        /// </summary>
        public System.String Score1 { get; set; }

        /// <summary>
        /// 分数2
        /// </summary>
        public System.String Score2 { get; set; }

        /// <summary>
        /// 分数3
        /// </summary>
        public System.String Score3 { get; set; }

        /// <summary>
        /// 分数4
        /// </summary>
        public System.String Score4 { get; set; }

        /// <summary>
        /// 分数5
        /// </summary>
        public System.String Score5 { get; set; }

        /// <summary>
        /// 分数6
        /// </summary>
        public System.String Score6 { get; set; }

        /// <summary>
        /// 分数7
        /// </summary>
        public System.String Score7 { get; set; }


        public void Create()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }

    /// <summary>
    /// 评分表
    /// </summary>
    public class ScoringDetailList
    {

        /// <summary>
        /// 评测人类型
        /// </summary>
        public System.String Type { get; set; }

        public System.String Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public System.String PId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public System.String UserName { get; set; }

        /// <summary>
        /// 评测总分
        /// </summary>
        public System.String ScoreTotal { get; set; }

        /// <summary>
        /// 分数1
        /// </summary>
        public System.String Score1 { get; set; }

        /// <summary>
        /// 分数2
        /// </summary>
        public System.String Score2 { get; set; }

        /// <summary>
        /// 分数3
        /// </summary>
        public System.String Score3 { get; set; }

        /// <summary>
        /// 分数4
        /// </summary>
        public System.String Score4 { get; set; }

        /// <summary>
        /// 分数5
        /// </summary>
        public System.String Score5 { get; set; }

        /// <summary>
        /// 分数6
        /// </summary>
        public System.String Score6 { get; set; }

        /// <summary>
        /// 分数7
        /// </summary>
        public System.String Score7 { get; set; }


        public void Create()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }

    public class scorelist
    {
        public string name { get; set; }
        public string zt1 { get; set; }
        public string zt2 { get; set; }
        public string zt3 { get; set; }
        public string zt4 { get; set; }
        public string ls1 { get; set; }
        public string ls2 { get; set; }
        public string ls3 { get; set; }
        public string ls4 { get; set; }
        public string jj1 { get; set; }
        public string jj2 { get; set; }
        public string jj3 { get; set; }
        public string jj4 { get; set; }
        public string zf1 { get; set; }
        public string zf2 { get; set; }
        public string zf3 { get; set; }
        public string zf4 { get; set; }
        public string lssj1 { get; set; }
        public string lssj2 { get; set; }
        public string lssj3 { get; set; }
        public string lssj4 { get; set; }
        public string jjzz1 { get; set; }
        public string jjzz2 { get; set; }
        public string jjzz3 { get; set; }
        public string jjzz4 { get; set; }
        public string wtzz1 { get; set; }
        public string wtzz2 { get; set; }
        public string wtzz3 { get; set; }
        public string wtzz4 { get; set; }
        public string gzzz1 { get; set; }
        public string gzzz2 { get; set; }
        public string gzzz3 { get; set; }
        public string gzzz4 { get; set; }
    }

    public class Griddto
    {
        public List<scorelist> list { get; set; }
        public int jtjw { get; set; }
        public int dwjw { get; set; }
        public int qtry { get; set; }
        public int total { get; set; }
    }
}

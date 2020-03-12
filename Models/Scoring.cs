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


        public void Create()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}

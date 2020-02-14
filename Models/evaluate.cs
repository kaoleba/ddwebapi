using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDWebApi
{
    [Table("evaluate")]
    /// <summary>
    /// 评分表
    /// </summary>
    public class evaluate
    {
        [ExplicitKey]
        /// <summary>
        /// 打分情况主键
        /// </summary>
        public System.String proposal_score_id { get; set; }

        /// <summary>
        /// 工作建议主键
        /// </summary>
        public System.String proposal_id { get; set; }

        /// <summary>
        /// 评测人主键
        /// </summary>
        public System.String evaluator_id { get; set; }

        /// <summary>
        /// 评测人名称
        /// </summary>
        public System.String evaluator_name { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public System.DateTime? submit_time { get; set; }

        /// <summary>
        /// 得分1
        /// </summary>
        public System.Int32? score1 { get; set; }

        /// <summary>
        /// 得分2
        /// </summary>
        public System.Int32? score2 { get; set; }

        /// <summary>
        /// 得分3
        /// </summary>
        public System.Int32? score3 { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public System.String def1 { get; set; }

        /// <summary>
        /// 备用字段2
        /// </summary>
        public System.String def2 { get; set; }

        /// <summary>
        /// 备用字段3
        /// </summary>
        public System.String def3 { get; set; }

        public void Create()
        {
            this.proposal_score_id = Guid.NewGuid().ToString();
            this.submit_time = DateTime.Now;
        }

        public void Modify()
        {
            this.submit_time = DateTime.Now;
        }
    }
}

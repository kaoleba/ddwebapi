using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWebApi
{
    [Table("data_analysis")]
    public class data_analysis
    {
        [ExplicitKey]
        /// <summary>
        /// 数据分析主键
        /// </summary>
        public System.String analysis_id { get; set; }

        /// <summary>
        /// 分析专题
        /// </summary>
        public System.String analysis_title { get; set; }

        /// <summary>
        /// 关键数据、结论及建议
        /// </summary>
        public System.String analysis_content { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public System.String creatorid { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public System.String creator { get; set; }

        /// <summary>
        /// 提报部门id
        /// </summary>
        public System.String analysis_deptid { get; set; }

        /// <summary>
        /// 提报部门
        /// </summary>
        public System.String analysis_dept { get; set; }

        /// <summary>
        /// 例会期数
        /// </summary>
        public System.String period { get; set; }

        /// <summary>
        /// 0:创建 1:提交 2:审核通过 -1:审核失败 3:参评
        /// </summary>
        public System.String state { get; set; }

        /// <summary>
        /// 客户端(PC DD)
        /// </summary>
        public System.String client { get; set; }

        /// <summary>
        /// 填报日期
        /// </summary>
        public System.DateTime? submit_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime? create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime? update_time { get; set; }

        /// <summary>
        /// 删除标志，1：删除，0：未删除。
        /// </summary>
        public System.String del_flag { get; set; }

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
            this.analysis_id = Guid.NewGuid().ToString();
            this.create_time = DateTime.Now;
            this.state = "0";
            this.del_flag = "0";
            this.client = "DD";
        }

        public void Modify()
        {
            this.update_time = DateTime.Now;
        }
    }


    [Table("data_analysis")]
    public class data_analysisDto
    {
        [ExplicitKey]
        /// <summary>
        /// 数据分析主键
        /// </summary>
        public System.String analysis_id { get; set; }

        /// <summary>
        /// 分析专题
        /// </summary>
        public System.String analysis_title { get; set; }

        /// <summary>
        /// 关键数据、结论及建议
        /// </summary>
        public System.String analysis_content { get; set; }

        /// <summary>
        /// 例会期数
        /// </summary>
        public System.String period { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime? update_time { get; set; }


        public void Modify()
        {
            this.update_time = DateTime.Now;
        }
    }

    [Table("data_analysis_score")]
    public class data_analysis_score
    {
        [ExplicitKey]
        /// <summary>
        /// 评分情况主键
        /// </summary>
        public System.String score_id { get; set; }

        /// <summary>
        /// 数据分析主键
        /// </summary>
        public System.String analysis_id { get; set; }

        /// <summary>
        /// 评测人主键
        /// </summary>
        public System.String reviewer_id { get; set; }

        /// <summary>
        /// 评测人名称
        /// </summary>
        public System.String reviewer { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public System.DateTime? submit_time { get; set; }

        /// <summary>
        /// 得分1
        /// </summary>
        public System.Single? score1 { get; set; }

        /// <summary>
        /// 得分2
        /// </summary>
        public System.Single? score2 { get; set; }

        /// <summary>
        /// 得分3
        /// </summary>
        public System.Single? score3 { get; set; }

        /// <summary>
        /// 得分4
        /// </summary>
        public System.Single? score4 { get; set; }

        /// <summary>
        /// 得分5
        /// </summary>
        public System.Single? score5 { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public System.Single? score_total { get; set; }

        /// <summary>
        /// 客户端(PC DD)
        /// </summary>
        public System.String client { get; set; }

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
            this.score_id = Guid.NewGuid().ToString();
            this.score_total = this.score1 + this.score2+this.score3+this.score4+this.score5;
            this.submit_time = DateTime.Now;
            this.client = "DD";
        }

        public void Modify()
        {
            this.client = "DD";
            this.score_total = this.score1 + this.score2 + this.score3 + this.score4 + this.score5;
            this.submit_time = DateTime.Now;
        }
    }


    public class rate_analysisDto
    {
        [ExplicitKey]
        /// <summary>
        /// 数据分析主键
        /// </summary>
        public System.String analysis_id { get; set; }

        /// <summary>
        /// 分析专题
        /// </summary>
        public System.String analysis_title { get; set; }

        /// <summary>
        /// 关键数据、结论及建议
        /// </summary>
        public System.String analysis_content { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public System.String creatorid { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public System.String creator { get; set; }

        /// <summary>
        /// 提报部门id
        /// </summary>
        public System.String analysis_deptid { get; set; }

        /// <summary>
        /// 提报部门
        /// </summary>
        public System.String analysis_dept { get; set; }

        /// <summary>
        /// 例会期数
        /// </summary>
        public System.String period { get; set; }

        /// <summary>
        /// 0:创建 1:提交 2:审核通过 -1:审核失败 3:参评
        /// </summary>
        public System.String state { get; set; }

        /// <summary>
        /// 客户端(PC DD)
        /// </summary>
        public System.String client { get; set; }

        /// <summary>
        /// 填报日期
        /// </summary>
        public System.DateTime? submit_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime? create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime? update_time { get; set; }

        /// <summary>
        /// 删除标志，1：删除，0：未删除。
        /// </summary>
        public System.String del_flag { get; set; }

        /// <summary>
        /// 评分情况主键
        /// </summary>
        public System.String score_id { get; set; }

        /// <summary>
        /// 评测人主键
        /// </summary>
        public System.String reviewer_id { get; set; }

        /// <summary>
        /// 评测人名称
        /// </summary>
        public System.String reviewer { get; set; }

        /// <summary>
        /// 得分1
        /// </summary>
        public System.Single? score1 { get; set; }

        /// <summary>
        /// 得分2
        /// </summary>
        public System.Single? score2 { get; set; }

        /// <summary>
        /// 得分3
        /// </summary>
        public System.Single? score3 { get; set; }

        /// <summary>
        /// 得分4
        /// </summary>
        public System.Single? score4 { get; set; }

        /// <summary>
        /// 得分5
        /// </summary>
        public System.Single? score5 { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public System.Single? score_total { get; set; }


    }


}

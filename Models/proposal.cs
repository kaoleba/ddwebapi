using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWebApi
{
    [Table("proposal")]
    /// <summary>
    /// 工作建议
    /// </summary>
    public class proposal
    {

        [ExplicitKey]
        /// <summary>
        /// 工作建议主键
        /// </summary>
        public String proposal_id { get; set; }

        /// <summary>
        /// 工作建议标题
        /// </summary>
        public String proposal_title { get; set; }

        /// <summary>
        /// 工作建议内容
        /// </summary>
        public String proposal_content { get; set; }

        /// <summary>
        /// 主办部门id
        /// </summary>
        public String host_deptid { get; set; }

        /// <summary>
        /// 主办部门
        /// </summary>
        public String host_dept { get; set; }

        /// <summary>
        /// 协办部门id
        /// </summary>
        public String assisting_deptid { get; set; }

        /// <summary>
        /// 协办部门
        /// </summary>
        public String assisting_dept { get; set; }

        /// <summary>
        /// 主管领导id
        /// </summary>
        public String host_userid { get; set; }

        /// <summary>
        /// 主管领导
        /// </summary>
        public String host_user { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public String creatorid { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public String creator { get; set; }

        /// <summary>
        /// 提报部门id
        /// </summary>
        public String proposal_deptid { get; set; }

        /// <summary>
        /// 提报部门
        /// </summary>
        public String proposal_dept { get; set; }

        /// <summary>
        /// 工作建议的解决方案
        /// </summary>
        public String proposal_project { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }

        /// <summary>
        /// 删除标志，1：删除，0：未删除。
        /// </summary>
        public String del_flag { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public String proposal_def1 { get; set; }

        /// <summary>
        /// 备用字段2
        /// </summary>
        public String proposal_def2 { get; set; }

        /// <summary>
        /// 备用字段3
        /// </summary>
        public String proposal_def3 { get; set; }

        /// <summary>
        /// 0:创建 1:提交 2:审核通过 -1:审核失败 3:参评
        /// </summary>
        public String state { get; set; }

        public void Create()
        {
            this.proposal_id = Guid.NewGuid().ToString();
            this.create_time = DateTime.Now;
            this.state = "0";
            this.del_flag = "0";
            this.proposal_def3 = "DD";
        }

        public void Modify()
        {
            this.update_time = DateTime.Now;
        }
    }


    [Table("proposal")]
    /// <summary>
    /// 工作建议
    /// </summary>
    public class proposalDto
    {
        [ExplicitKey]
        /// <summary>
        /// 工作建议主键
        /// </summary>
        public String proposal_id { get; set; }

        /// <summary>
        /// 工作建议标题
        /// </summary>
        public String proposal_title { get; set; }

        /// <summary>
        /// 工作建议内容
        /// </summary>
        public String proposal_content { get; set; }

        /// <summary>
        /// 主办部门id
        /// </summary>
        public String host_deptid { get; set; }

        /// <summary>
        /// 主办部门
        /// </summary>
        public String host_dept { get; set; }

        /// <summary>
        /// 协办部门id
        /// </summary>
        public String assisting_deptid { get; set; }

        /// <summary>
        /// 协办部门
        /// </summary>
        public String assisting_dept { get; set; }

        /// <summary>
        /// 主管领导id
        /// </summary>
        public String host_userid { get; set; }

        /// <summary>
        /// 主管领导
        /// </summary>
        public String host_user { get; set; }


        /// <summary>
        /// 提报部门id
        /// </summary>
        public String proposal_deptid { get; set; }

        /// <summary>
        /// 提报部门
        /// </summary>
        public String proposal_dept { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public String proposal_def1 { get; set; }


        public void Modify()
        {
            this.update_time = DateTime.Now;
        }

    }


    [Table("proposal")]
    /// <summary>
    /// 工作建议
    /// </summary>
    public class ratelistDto
    {

        [ExplicitKey]
        /// <summary>
        /// 工作建议主键
        /// </summary>
        public String proposal_id { get; set; }

        /// <summary>
        /// 工作建议标题
        /// </summary>
        public String proposal_title { get; set; }

        /// <summary>
        /// 工作建议内容
        /// </summary>
        public String proposal_content { get; set; }

        /// <summary>
        /// 主办部门id
        /// </summary>
        public String host_deptid { get; set; }

        /// <summary>
        /// 主办部门
        /// </summary>
        public String host_dept { get; set; }

        /// <summary>
        /// 协办部门id
        /// </summary>
        public String assisting_deptid { get; set; }

        /// <summary>
        /// 协办部门
        /// </summary>
        public String assisting_dept { get; set; }

        /// <summary>
        /// 主管领导id
        /// </summary>
        public String host_userid { get; set; }

        /// <summary>
        /// 主管领导
        /// </summary>
        public String host_user { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public String creatorid { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public String creator { get; set; }

        /// <summary>
        /// 提报部门id
        /// </summary>
        public String proposal_deptid { get; set; }

        /// <summary>
        /// 提报部门
        /// </summary>
        public String proposal_dept { get; set; }

        /// <summary>
        /// 工作建议的解决方案
        /// </summary>
        public String proposal_project { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public String evaluator_id { get; set; }

        /// <summary>
        /// 备用字段2
        /// </summary>
        public String proposal_score_id { get; set; }


        /// <summary>
        /// 0:创建 1:提交 2:审核通过 -1:审核失败 3:参评
        /// </summary>
        public String state { get; set; }


        public double score1 { get; set; }

        public double score2 { get; set; }

        public void Modify()
        {
            this.update_time = DateTime.Now;
        }

    }


    /**
     * 打分情况
     * */
    public class ScoreList {
        public string proposal_dept { get; set; }
        public decimal score { get; set; }
        public int monthorder { get; set; }
    }
}
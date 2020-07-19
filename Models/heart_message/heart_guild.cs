using Dapper.Contrib.Extensions;
using System;

namespace DDWebApi
{
    [Table("heart_guild")]
    /// <summary>
    /// 职工帮办
    /// </summary>
    public class heart_guild
    {
        /// <summary>
        /// 职工帮办
        /// </summary>
        public heart_guild()
        {
        }

        [ExplicitKey]
        /// <summary>
        /// 主键
        /// </summary>
        public System.String id { get; set; }

        /// <summary>
        /// dingid
        /// </summary>
        public System.String dingid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public System.String username { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public System.String mobile { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public System.String code { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public System.String company { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public System.String dept { get; set; }

        /// <summary>
        /// 问题类别
        /// </summary>
        public System.String type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public System.String title { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public System.String content { get; set; }

        /// <summary>
        /// 落实dingid
        /// </summary>
        public System.String zpdingid { get; set; }

        /// <summary>
        /// 落实人
        /// </summary>
        public System.String zpusername { get; set; }

        /// <summary>
        /// 处理dingid
        /// </summary>
        public System.String cldingid { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public System.String clusername { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public System.DateTime? cldate { get; set; }

        /// <summary>
        /// 落实措施
        /// </summary>
        public System.String solve { get; set; }

        /// <summary>
        /// 落实时间
        /// </summary>
        public System.DateTime? solvedate { get; set; }

        /// <summary>
        /// 预计落实时间
        /// </summary>
        public System.DateTime? zpdate { get; set; }

        /// <summary>
        /// 状态0 未指派 1 处理中 2已完成
        /// </summary>
        public System.Int32? state { get; set; }

        /// <summary>
        /// 落实人单位
        /// </summary>
        public System.String cldept { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public System.Int32? score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String replyid { get; set; }
        

        public DateTime createdate { get; set; }

        public void Create()
        {
            this.id = Guid.NewGuid().ToString();
            this.createdate = DateTime.Now;
            this.state = 0;
        }
    }
}

using Dapper.Contrib.Extensions;
using System;

namespace DDWebApi
{
    [Table("heart_message")]
    /// <summary>
    /// 
    /// </summary>
    public class heart_message
    {
        /// <summary>
        /// 
        /// </summary>
        public heart_message()
        {
        }

        [ExplicitKey]
        /// <summary>
        /// 主键
        /// </summary>
        public System.String id { get; set; }

        /// <summary>
        /// 留言标题
        /// </summary>
        public System.String title { get; set; }

        /// <summary>
        /// 留言信息
        /// </summary>
        public System.String message { get; set; }

        /// <summary>
        /// 留言信息
        /// </summary>
        public System.String dingid { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public System.String ip { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime? createdate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime? updatedate { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public System.String type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public System.Int32? state { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public System.String origin { get; set; }

        /// <summary>
        /// 回复意见
        /// </summary>
        public System.String replymessage { get; set; }

        /// <summary>
        /// 转办意见
        /// </summary>
        public System.String convertmessage { get; set; }

        /// <summary>
        /// 最终意见
        /// </summary>
        public System.String reply { get; set; }

        /// <summary>
        /// 待办ID
        /// </summary>
        public System.String replyid { get; set; }

        /// <summary>
        /// 处理单位
        /// </summary>
        public System.String dept { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public System.String username { get; set; }

        /// <summary>
        /// 处理人工号
        /// </summary>
        public System.String code { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public System.DateTime? replaydate { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public System.DateTime? publicdate { get; set; }

        /// <summary>
        /// 当前处理人
        /// </summary>
        public System.String currentcode { get; set; }

        /// <summary>
        /// 下一节点处理人
        /// </summary>
        public System.String nextcode { get; set; }


        public void Create()
        {
            this.id = Guid.NewGuid().ToString();
            this.origin = "DD";
            this.createdate = DateTime.Now;
            this.updatedate = DateTime.Now;
            this.state = 0;
        }
    }

}

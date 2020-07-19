using Dapper.Contrib.Extensions;
using System;

namespace DDWebApi
{
    [Table("heart_flow")]
    /// <summary>
    /// 
    /// </summary>
    public class heart_flow
    {
        /// <summary>
        /// 
        /// </summary>
        public heart_flow()
        {
        }

        [ExplicitKey]
        /// <summary>
        /// 主键
        /// </summary>
        public System.String id { get; set; }

        /// <summary>
        /// 承办消息ID
        /// </summary>
        public System.String messageid { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public System.String code { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public System.String name { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public System.DateTime? createdate { get; set; }

        /// <summary>
        /// 不同意
        /// </summary>
        public System.String refuse { get; set; }

        public void Create()
        {
            this.id = Guid.NewGuid().ToString();
            this.createdate = DateTime.Now;
        }
    }
}

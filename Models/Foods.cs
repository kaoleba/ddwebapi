using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDWebApi
{
    [Table("foods_evaluate")]
    /// <summary>
    /// 
    /// </summary>
    public class foods_evaluate
    {
        /// <summary>
        /// 
        /// </summary>
        public foods_evaluate()
        {
        }

        [ExplicitKey]
        /// <summary>
        /// 主键
        /// </summary>
        public System.String Id { get; set; }

        /// <summary>
        /// 食品id
        /// </summary>
        public System.String foodid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public System.String userid { get; set; }

        /// <summary>
        /// 评价1
        /// </summary>
        public System.Int32? value1 { get; set; }

        /// <summary>
        /// 评价2
        /// </summary>
        public System.Int32? value2 { get; set; }

        public void Create()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }

    [Table("foods_user")]
    /// <summary>
    /// 
    /// </summary>
    public class foods_user
    {
        /// <summary>
        /// 
        /// </summary>
        public foods_user()
        {
        }


        /// <summary>
        /// 问卷ID
        /// </summary>
        public System.String questionid { get; set; }

        /// <summary>
        /// 钉钉id
        /// </summary>
        public System.String userid { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public System.String usercode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public System.String username { get; set; }

        /// <summary>
        /// 评分1
        /// </summary>
        public System.Int32? value1 { get; set; }

        /// <summary>
        /// 评分2
        /// </summary>
        public System.Int32? value2 { get; set; }

        /// <summary>
        /// 评分3
        /// </summary>
        public System.Int32? value3 { get; set; }

        /// <summary>
        /// 意见1
        /// </summary>
        public System.String remark1 { get; set; }

        /// <summary>
        /// 意见2
        /// </summary>
        public System.String remark2 { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime? CreateDate { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public System.DateTime? SubmitDate { get; set; }

        [ExplicitKey]
        /// <summary>
        /// 主键
        /// </summary>
        public System.String Id { get; set; }
    }

    /// <summary>
    /// 问卷表
    /// </summary>
    [Table("qustion")]
    public class qustion
    {
        #region 实体成员 
        /// <summary> 
        /// Id 
        /// </summary> 
        /// <returns></returns> 
        [ExplicitKey]
        public string Id { get; set; }
        /// <summary> 
        /// 问卷标题 
        /// </summary> 
        /// <returns></returns> 
        public string Title { get; set; }
        /// <summary> 
        /// 问卷日期 
        /// </summary> 
        /// <returns></returns> 
        public DateTime? QustionDate { get; set; }
        /// <summary> 
        /// 发布状态 
        /// </summary> 
        /// <returns></returns> 
        public int State { get; set; }
        /// <summary> 
        /// 结束日期 
        /// </summary> 
        /// <returns></returns> 
        public DateTime? EndTime { get; set; }
        /// <summary> 
        /// 创建日期 
        /// </summary> 
        /// <returns></returns> 
        public DateTime? CreateDate { get; set; }

        /// <summary> 
        /// 创建日期 
        /// </summary> 
        /// <returns></returns> 
        public string Description { get; set; }

        #endregion

    }
}

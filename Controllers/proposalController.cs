using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class proposalController : ControllerBase
    {
        private DBHelper db = new DBHelper();

        // GET api/proposal
        [HttpGet]
        [EnableCors("any")]
        public ActionResult<IEnumerable<proposal>> Get(int? page, string state, string ny, string deptid)
        {
            try
            {
                if (page is null)
                    return null;
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select * from proposal  where ifnull(del_flag,0)=0");

                if (state == "参评建议")
                    sql.Append(" and state='3' ");
                if (!string.IsNullOrEmpty(deptid))
                {
                    sql.Append(" and proposal_deptid=?proposal_deptid");
                    pars.Add("proposal_deptid", deptid);
                }
                if (!string.IsNullOrEmpty(ny))
                {
                    string StartDate = ny + "/01";
                    string EndDate = Convert.ToDateTime(StartDate).AddMonths(1).ToString("yyyy/MM/dd");
                    sql.Append(" and create_time>='" + StartDate + "' and create_time<'" + EndDate + "'");
                }
                LogHelper.Debug("查询建议列表" + sql.ToString());
                return db.GetPageList<proposal>(sql.ToString(), pars, "create_time", "desc", Convert.ToInt32(page), 20);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取建议信息异常：" + ex.Message);
                return null;
            }
        }

        [HttpGet]
        [EnableCors("any")]
        [Route("GetRateList")]
        public ActionResult<IEnumerable<ratelistDto>> GetRateList(int? page, string state, string ny, string evaluator_id)
        {
            try
            {
                if (page is null)
                    return null;
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select a.*,b.evaluator_id ,b.proposal_score_id ,b.score1,b.score2 " +
                    " from proposal a  left  join evaluate b on a.proposal_id=b.proposal_id  and (evaluator_id=?evaluator_id or evaluator_id is null) where state='3' ");
                if (state == "未评建议")
                    sql.Append(" and b.evaluator_id is null");
                if (state == "已评建议")
                    sql.Append(" and b.evaluator_id is not null");
                pars.Add("evaluator_id", evaluator_id);
                if (!string.IsNullOrEmpty(ny))
                {
                    string StartDate = ny + "/01";
                    string EndDate = Convert.ToDateTime(StartDate).AddMonths(1).ToString("yyyy/MM/dd");
                    sql.Append(" and create_time>='" + StartDate + "' and create_time<'" + EndDate + "'");
                }
                LogHelper.Debug("查询评分列表" + sql.ToString());
                return db.GetPageList<ratelistDto>(sql.ToString(), pars, "create_time", "desc", Convert.ToInt32(page), 20);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取打分列表异常：" + ex.Message);
                return null;
            }
        }

        // GET  api/proposal/5
        [HttpGet("{id}")]
        [EnableCors("any")]
        public ActionResult<proposal> Get(string id)
        {
            var pro = db.GetEntityById<proposal>(id);
            return pro;
        }

        [HttpPost]
        [EnableCors("any")]
        [Route("SaveSore")]
        public string SaveSore(evaluate eva)
        {
            try
            {
                if (string.IsNullOrEmpty(eva.proposal_score_id))
                {
                    eva.Create();
                    db.Insert<evaluate>(eva);
                }
                else
                {
                    eva.Modify();
                    db.Update<evaluate>(eva);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("打分异常：" + ex.Message);
                return ex.Message;
            }
        }

        [HttpPost]
        [EnableCors("any")]
        public string Post(proposal prop)
        {
            try
            {
                if (string.IsNullOrEmpty(prop.proposal_id))
                {
                    prop.Create();
                    db.Insert<proposal>(prop);
                }
                else
                {
                    proposalDto des = new proposalDto();
                    var desDto = prop.MapTo(des);
                    desDto.Modify();
                    db.Update<proposalDto>(desDto);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("提交建议异常：" + ex.Message);
                return ex.Message;
            }
        }

        // DELETE api/proposal/5
        [HttpDelete("{id}")]
        [EnableCors("any")]
        public string Delete(string id)
        {
            try
            {
                DynamicParameters pars = new DynamicParameters();
                pars.Add("id", id);
                db.ExecuteSql("UPDATE proposal SET del_flag= 1 WHERE proposal_id=?id and state='0'", pars);
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除建议异常：" + ex.Message);
                return ex.Message;
            }
        }

        // DELETE api/proposal/UpdateState
        [HttpGet]
        [Route("UpdateState")]
        [EnableCors("any")]
        public string UpdateState(string id, string state)
        {
            try
            {
                //提交更改状态为1，参评判断当月是否已经有参评建议，一月一次
                DynamicParameters pars = new DynamicParameters();
                pars.Add("id", id);
                pars.Add("state", state);
                if (state == "3")
                {
                    proposal pro = db.GetEntityById<proposal>(id);
                    //if (pro.state != "1")
                    //    throw new Exception("只有提交状态建议能够参评！");
                    DynamicParameters parsquery = new DynamicParameters();
                    parsquery.Add("proposal_deptid", pro.proposal_deptid);
                    parsquery.Add("Create_Time", pro.create_time, System.Data.DbType.DateTime);
                    var resnum = db.QueryFirstOrDefault<int>("select count(1) from proposal where state='3' and date_format(Create_Time, '%Y-%m') = date_format(?Create_Time, '%Y-%m') and proposal_deptid=?proposal_deptid", parsquery);
                    if (resnum >= 1)
                        throw new Exception("每月只能一条建议参评！");
                }
                db.ExecuteSql("UPDATE proposal SET state= ?state WHERE proposal_id=?id ", pars);
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新建议状态异常：" + ex.Message);
                return ex.Message;
            }
        }

        //判断提报建议是否全部完成打分
        //date:yyyy-MM
        public bool CheckEvaluateDone(string deptid, string date,int count)
        {
            string sql = @"select count(1)  from proposal a left join evaluate b
                            on a.proposal_id=b.proposal_id
                            where state='3' and date_format(create_time,'%Y-%m')='" + date + "'";
            if (deptid != "")
            {
                sql += " and proposal_deptid='" + deptid + "'";
            }
            if (new DBHelper().GetList<int>(sql).Count < count)
            {
                return false;
            }
            return true;
        }

        //获取领导班子钉钉ID
        public List<string> LeaderDDID()
        {
            DBHelper ssdb = new DBHelper("MSSQLCon");
            List<string> list = ssdb.GetList<string>("select DingTalk from msgcenter_person where DeptCode='L10101' and gwzt='01' and DingTalk is not null");//获取领导班子DingTalk列表
            return list;
        }

        //获取当月得分排名
        //todo 验证工作建议是否所有领导均打分
        [HttpGet]
        [Route("MonthScoreList")]
        [EnableCors("any")]
        public ActionResult<List<ScoreList>> MonthScoreList()
        {
            List<ScoreList> list = new List<ScoreList>();
            if (!CheckEvaluateDone("", DateTime.Now.ToString("yyyy-MM"), LeaderDDID().Count))
            {
                return list;
            }
            try
            {
                //陈琪 2020/02/18 更改  将Month(create_time)=Month(now()) 更改为 date_format(create_time, '%Y-%m') = date_format(now(), '%Y-%m')
                //原因 Month(201806)=Month(201906)
                string sql = @"select proposal_dept,ROUND(avg(score),2) score from (select proposal_id,proposal_dept from proposal 
                where state='3' and date_format(create_time, '%Y-%m') = date_format(now(), '%Y-%m')) a left join    
                (select proposal_id,ROUND(avg(score3),2) score from evaluate group by proposal_id) b 
                on a.proposal_id=b.proposal_id group by proposal_dept order by score desc";
                list = db.GetList<ScoreList>(sql);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return list;
        }

        //获取部门得分情况
        //todo 验证工作建议是否所有领导均打分
        [HttpGet]
        [Route("DeptScoreList")]
        [EnableCors("any")]
        public ActionResult<List<ScoreList>> DeptScoreList(string deptId)
        {
            List<ScoreList> list = new List<ScoreList>();
            try
            {
                string sql = @"select proposal_dept,ROUND(avg(score),2) score,monthorder from 
                    (select proposal_id,proposal_dept,month(create_time) monthorder from proposal where state='3'
                    and year(create_time)=year(now()) and proposal_deptid='" + deptId + @"') a left join 
                    (select proposal_id,ROUND(avg(score3),2) score from evaluate group by proposal_id) b 
                    on a.proposal_id=b.proposal_id group by monthorder order by monthorder desc";
                list = db.GetList<ScoreList>(sql);
                int count = LeaderDDID().Count;
                for (int i = 0; i <= list.Count; i++)
                {
                    if (!CheckEvaluateDone(deptId, DateTime.Now.ToString("yyyy-")+ list[i].monthorder.ToString().PadLeft(2, '0'),count))
                    {
                        list[i].score = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return list;
        }
    }
}
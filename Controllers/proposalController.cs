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
        public ActionResult<IEnumerable<proposal>> Get(int page, string state, string ny, string deptid)
        {
            try
            {
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
                return db.GetPageList<proposal>(sql.ToString(), pars, "create_time", "desc", page, 20);
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
        public ActionResult<IEnumerable<ratelistDto>> GetRateList(int page, string state, string ny, string evaluator_id)
        {
            try
            {
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select a.*,b.evaluator_id ,b.proposal_score_id ,b.score1 " +
                    " from proposal a  left  join evaluate b on a.proposal_id=b.proposal_id where state='3' ");

                if (state == "未评建议")
                    sql.Append(" and b.evaluator_id is null");
                if (state == "已评建议")
                    sql.Append(" and b.evaluator_id is not null");

                if (!string.IsNullOrEmpty(evaluator_id))
                {
                    sql.Append(" and (evaluator_id=?evaluator_id or evaluator_id is null)");
                    pars.Add("evaluator_id", evaluator_id);
                }

                if (!string.IsNullOrEmpty(ny))
                {
                    string StartDate = ny + "/01";
                    string EndDate = Convert.ToDateTime(StartDate).AddMonths(1).ToString("yyyy/MM/dd");
                    sql.Append(" and create_time>='" + StartDate + "' and create_time<'" + EndDate + "'");
                }
                return db.GetPageList<ratelistDto>(sql.ToString(), pars, "create_time", "desc", page, 20);
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
                    if (pro.state != "1")
                        throw new Exception("只有提交状态建议能够参评！");
                    DynamicParameters parsquery = new DynamicParameters();
                    parsquery.Add("proposal_deptid", pro.proposal_deptid);
                    parsquery.Add("Create_Time", pro.create_time, System.Data.DbType.DateTime);
                    var resnum = db.QueryFirstOrDefault<int>("select count(1) from proposal where state='3' and TIMESTAMPDIFF(MONTH,Create_Time ,?Create_Time)=0 and proposal_deptid=?proposal_deptid", parsquery);
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

        //获取当月得分排名
        [HttpGet]
        [Route("MonthScoreList")]
        [EnableCors("any")]
        public ActionResult<List<ScoreList>> MonthScoreList()
        {
            List<ScoreList> list = new List<ScoreList>();
            try
            {
                string sql = @"select proposal_dept,ROUND(avg(score),2) score from (select proposal_id,proposal_dept from proposal where state='3' and month(create_time)=month(now())) a left join 
(select proposal_id,ROUND(avg(score1),2) score from evaluate group by proposal_id) b on a.proposal_id=b.proposal_id group by proposal_dept order by score desc";
                list = db.GetList<ScoreList>(sql);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return list;
        }

        //获取部门得分排名
        [HttpGet]
        [Route("DeptScoreList")]
        [EnableCors("any")]
        public ActionResult<List<ScoreList>> DeptScoreList()
        {
            List<ScoreList> list = new List<ScoreList>();
            try
            {
                string sql = @"select proposal_dept,ROUND(avg(score),2) score,monthorder from (select proposal_id,proposal_dept,month(create_time) monthorder from proposal where state='3' and year(create_time)=year(now()) and proposal_dept='大数据中心') a left join 
(select proposal_id,ROUND(avg(score1),2) score from evaluate group by proposal_id) b on a.proposal_id=b.proposal_id group by monthorder order by monthorder desc";
                list = db.GetList<ScoreList>(sql);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            return list;
        }

    }
}
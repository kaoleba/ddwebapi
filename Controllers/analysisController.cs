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
    public class analysisController : ControllerBase
    {
        private DBHelper db = new DBHelper();

        // GET api/analysis
        [HttpGet]
        [EnableCors("any")]
        public ActionResult<IEnumerable<data_analysis>> Get(int? page, string state, string ny, string deptid)
        {
            try
            {
                if (page is null)
                    return null;
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select * from data_analysis  where ifnull(del_flag,0)=0");

                if (state == "参评分析")
                    sql.Append(" and state='3' ");
                if (!string.IsNullOrEmpty(deptid))
                {
                    sql.Append(" and analysis_deptid=?analysis_deptid");
                    pars.Add("analysis_deptid", deptid);
                }
                if (!string.IsNullOrEmpty(ny))
                {
                    string StartDate = ny + "/01";
                    string EndDate = Convert.ToDateTime(StartDate).AddMonths(1).ToString("yyyy/MM/dd");
                    sql.Append(" and create_time>='" + StartDate + "' and create_time<'" + EndDate + "'");
                }
                LogHelper.Debug("查询数据分析列表" + sql.ToString());
                return db.GetPageList<data_analysis>(sql.ToString(), pars, "create_time", "desc", Convert.ToInt32(page), 20);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取数据分析信息异常：" + ex.Message);
                return null;
            }
        }

        [HttpGet]
        [EnableCors("any")]
        [Route("GetRateList")]
        public ActionResult<IEnumerable<rate_analysisDto>> GetRateList(int? page, string state, string ny, string evaluator_id)
        {
            try
            {
                if (page is null)
                    return null;
                DynamicParameters pars = new DynamicParameters();
                StringBuilder sql = new StringBuilder("select a.*,b.reviewer_id ,b.score_id ,b.score1,b.score2, b.score3,b.score4,b.score5" +
                    " from data_analysis a  left  join data_analysis_score b on a.analysis_id=b.analysis_id  and (reviewer_id=?evaluator_id or reviewer_id is null) where state='3' ");
                if (state == "未评分析")
                    sql.Append(" and b.reviewer_id is null");
                if (state == "已评分析")
                    sql.Append(" and b.reviewer_id is not null");
                pars.Add("evaluator_id", evaluator_id);
                if (!string.IsNullOrEmpty(ny))
                {
                    string StartDate = ny + "/01";
                    string EndDate = Convert.ToDateTime(StartDate).AddMonths(1).ToString("yyyy/MM/dd");
                    sql.Append(" and create_time>='" + StartDate + "' and create_time<'" + EndDate + "'");
                }
                LogHelper.Debug("查询数据分析评分列表" + sql.ToString());
                return db.GetPageList<rate_analysisDto>(sql.ToString(), pars, "create_time", "desc", Convert.ToInt32(page), 20);
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取数据分析列表异常：" + ex.Message);
                return null;
            }
        }

        // GET  api/analysis/5
        [HttpGet("{id}")]
        [EnableCors("any")]
        public ActionResult<data_analysis> Get(string id)
        {
            var pro = db.GetEntityById<data_analysis>(id);
            return pro;
        }


        [HttpPost]
        [EnableCors("any")]
        [Route("SaveSore")]
        public string SaveSore(data_analysis_score da)
        {
            try
            {
                if (string.IsNullOrEmpty(da.score_id))
                {
                    da.Create();
                    db.Insert<data_analysis_score>(da);
                }
                else
                {
                    da.Modify();
                    db.Update<data_analysis_score>(da);
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
        public string Post(data_analysis prop)
        {
            try
            {
                if (string.IsNullOrEmpty(prop.analysis_id))
                {
                    prop.Create();
                    db.Insert<data_analysis>(prop);
                }
                else
                {
                    data_analysisDto des = new data_analysisDto();
                    var desDto = prop.MapTo(des);
                    desDto.Modify();
                    db.Update<data_analysisDto>(desDto);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("提交数据分析异常：" + ex.Message);
                return ex.Message;
            }
        }

        // DELETE api/analysis/5
        [HttpDelete("{id}")]
        [EnableCors("any")]
        public string Delete(string id)
        {
            try
            {
                DynamicParameters pars = new DynamicParameters();
                pars.Add("id", id);
                db.ExecuteSql("UPDATE data_analysis SET del_flag= 1 WHERE analysis_id=?id and state='0'", pars);
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除数据分析异常：" + ex.Message);
                return ex.Message;
            }
        }

        // DELETE api/analysis/UpdateState
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
                    data_analysis pro = db.GetEntityById<data_analysis>(id);
                    //if (pro.state != "1")
                    //    throw new Exception("只有提交状态建议能够参评！");
                    DynamicParameters parsquery = new DynamicParameters();
                    parsquery.Add("analysis_deptid", pro.analysis_deptid);
                    parsquery.Add("Create_Time", pro.create_time, System.Data.DbType.DateTime);
                    var resnum = db.QueryFirstOrDefault<int>("select count(1) from data_analysis where state='3' and date_format(Create_Time, '%Y-%m') = date_format(?Create_Time, '%Y-%m') and analysis_deptid=?analysis_deptid", parsquery);
                    if (resnum >= 1)
                        throw new Exception("每月只能一条数据分析参评！");
                }
                db.ExecuteSql("UPDATE data_analysis SET state= ?state WHERE analysis_id=?id ", pars);
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error("更改数据分析异常：" + ex.Message);
                return ex.Message;
            }
        }

    }
}
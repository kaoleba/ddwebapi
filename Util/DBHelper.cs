using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using System.Data.Common;
using System.Text;
using System;

namespace DDWebApi
{
    public class DBFactory
    {
        private static string ConnectionString;

        public static IDbConnection CreateDb(string DBName)
        {
            ConnectionString = ConfigurationHelper.GetConStr(DBName);
            if (DBName.ToUpper().IndexOf("ORACLE") >= 0)
                return new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString);
            if (DBName.ToUpper().IndexOf("MSSQL") >= 0)
                return new System.Data.SqlClient.SqlConnection(ConnectionString);
            if (DBName.ToUpper().IndexOf("MYSQL") >= 0)
                return new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            return new System.Data.SqlClient.SqlConnection(ConnectionString);
        }
    }

    public class DBHelper
    {
        private int? commandTimeout = 60;
        private IDbTransaction isOpenTrans { get; set; }
        public string DBName;


        public  DBHelper()
        {
            this.DBName = "MYSQLCon";
        }

        public DBHelper(string Name)
        {
            this.DBName = Name;
        }

        public IDbConnection CreateDbConnection()
        {
            return DBFactory.CreateDb(DBName);
        }

        public IDbTransaction BeginTransaction()
        {
            IDbConnection db = CreateDbConnection();
            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }
            isOpenTrans = db.BeginTransaction();
            return isOpenTrans;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (isOpenTrans != null)
            {
                this.isOpenTrans.Commit();
                this.isOpenTrans.Connection.Close();
                this.isOpenTrans.Connection.Dispose();
                this.isOpenTrans = null;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (isOpenTrans != null)
            {
                this.isOpenTrans.Rollback();
                this.isOpenTrans.Connection.Close();
                this.isOpenTrans.Connection.Dispose();
                this.isOpenTrans = null;
            }
        }

        public List<T> GetList<T>(string sql)
        {
            using (IDbConnection db = CreateDbConnection())
            {
                db.Open();
                return db.Query<T>(sql).ToList();
            }
        }

        public List<T> GetList<T>(string sql, object param = null)
        {
            using (IDbConnection db = CreateDbConnection())
            {
                db.Open();
                return db.Query<T>(sql, param).ToList();
            }
        }

        public T GetEntity<T>(string sql)
        {
            using (IDbConnection db = CreateDbConnection())
            {
                db.Open();
                return db.QueryFirstOrDefault<T>(sql);
            }
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
                using (IDbConnection db = CreateDbConnection())
                {
                    db.Open();
                    return db.QueryFirstOrDefault<T>(sql, param);
                }
        }

        public int ExecuteSql(string sql,object param=null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (commandTimeout == null)
                commandTimeout = this.commandTimeout;
            if (transaction == null || transaction.Connection == null)
                using (IDbConnection db = CreateDbConnection())
                {
                    db.Open();
                    return db.Execute(sql, param, transaction, commandTimeout);
                }
            else
            {
                return transaction.Connection.Execute(sql, param, null,commandTimeout);
            }
        }

        public T GetEntityById<T>(object id) where T : class
        {
            using (IDbConnection db = CreateDbConnection())
            {
                db.Open();
                return db.Get<T>(id);
            }
        }

        public void Insert<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (commandTimeout == null)
                commandTimeout = this.commandTimeout;
            if (transaction == null || transaction.Connection == null)
                using (IDbConnection db = CreateDbConnection())
                {
                    db.Open();
                    db.Insert(entity, transaction, commandTimeout);
                }
            else
            {
                transaction.Connection.Insert(entity, null, commandTimeout);
            }
        }

        public void Update<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (commandTimeout == null)
                commandTimeout = this.commandTimeout;
            if (transaction == null || transaction.Connection == null)
                using (IDbConnection db = CreateDbConnection())
                {
                    db.Open();
                    db.Update(entity, transaction, commandTimeout);
                }
            else
            {
                transaction.Connection.Update(entity, null, commandTimeout);
            }
        }

        public void Delete<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (commandTimeout == null)
                commandTimeout = this.commandTimeout;
            if (transaction == null || transaction.Connection == null)
                using (IDbConnection db = CreateDbConnection())
                {
                    db.Open();
                    db.Delete(entity, transaction, commandTimeout);
                }
            else
            {
                transaction.Connection.Delete(entity, transaction, commandTimeout);
            }
        }


        public void Delete<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (commandTimeout == null)
                commandTimeout = this.commandTimeout;
            T entity = GetEntityById<T>(id);
            if (entity == null)
                return;
            if (transaction == null || transaction.Connection == null)
                using (IDbConnection db = CreateDbConnection())
                {
                    db.Open();
                    db.Delete(entity, transaction, commandTimeout);
                }
            else
            {
                transaction.Connection.Delete(entity, transaction, commandTimeout);
            }
        }

        public List<T> GetPageList<T>(string sql, Object param, string orderField, string orderType, int pageIndex, int pageSize)
        {
            using (IDbConnection db = CreateDbConnection())
            {
                db.Open();
                StringBuilder sb = new StringBuilder();
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                string OrderBy = "";

                if (!string.IsNullOrEmpty(orderField))
                    OrderBy = " Order By " + orderField + " " + orderType + "";
                else
                    OrderBy = " Order By (select 0)";
                sb.Append(sql + OrderBy);
                sb.Append(" limit " + num + "," + pageSize + "");
                //count =Convert.ToInt32(db.ExecuteScalar("Select Count(1) From (" + sql + ") As t", param));
                return db.Query<T>(sb.ToString(), param).ToList();
            }
        }

    }
}


//============================================================
// Producnt name:		TenXiang.Code
// Coded by:			hotboart
// Auto generated at: 	2020/5/5 星期二 16:03:00
//============================================================



using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Net.Itcast.CN.DAL
{
     public abstract class DBHelper<T>
    {
        #region 子类继承实现，创建对象
        protected abstract T CreateSingle(SqlDataReader reader);
        #endregion
        private SqlConnection GetConnection()
        {
			
            string connectionString = @"server=30YU1CEWFS3390J\SQLEXPRESS;uid=sa;pwd=1234;database=Shoping";
			//connectionString =ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        #region 基本执行方法
        protected int ExecNonQuery(string sql, params SqlParameter[] values)
        {
            using (SqlConnection scon = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, scon);
                cmd.Parameters.AddRange(values);
                return cmd.ExecuteNonQuery();
            }
        }

        protected object getScalar(string safeSql, params SqlParameter[] values)
        {
            using (SqlConnection scon = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(safeSql, scon);
                
                cmd.Parameters.AddRange(values);
                return cmd.ExecuteScalar();
            }
        }
        protected IList<T> GetList(string sql, params SqlParameter[] values)
        {
            using(SqlConnection scon = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, scon);
                cmd.Parameters.AddRange(values);
                
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    IList<T> list = new List<T>();
                    while (reader.Read())
                    {
                        T temp = CreateSingle(reader);
                        list.Add(temp);
                    }
                    return list;
                }
            }
        }
        protected T GetOne(string sql, params SqlParameter[] values)
        {
            using (SqlConnection scon = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, scon);
                cmd.Parameters.AddRange(values);

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        T temp = CreateSingle(reader);
                        return temp;
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }
        protected void GetPaging(PageingModel<T> Paging, string sql, params SqlParameter[] values)
        {
            string PagingSql = @"SELECT * FROM ( select ROW_NUMBER() over ( ORDER BY " + Paging.Order + ") AS NO ," +
                               sql.Substring(sql.IndexOf(" ")) +
                               "     ) t WHERE t.no between " + ((Paging.PageIndex - 1) * Paging.PageSize + 1) + " and " + Paging.PageIndex * Paging.PageSize;

            string CalcCountSql = "SELECT Count(*) FROM (" + sql + ") T";

            Paging.RecordCount = (int)getScalar(CalcCountSql,values);
            Paging.List = GetList(PagingSql, values);
        }
        #endregion

        #region 数据类型处理
        protected static int GetInt(object obj,int defVal=0)
        {
            if (obj is DBNull)
                return defVal;
            else
                return (int)obj;
        }
        protected static float GetFloat(object obj, float defVal = 0)
        {
            if (obj is DBNull)
                return defVal;
            else
                return (float)obj;
        }
        protected static double GetDouble(object obj, double defVal = 0)
        {
            if (obj is DBNull)
                return defVal;
            else
                return (double)obj;
        }
        protected static long GetLong(object obj, long defVal = 0)
        {
            if (obj is DBNull)
                return defVal;
            else
                return (long)obj;
        }
        protected static decimal GetDecimal(object obj, decimal defVal = 0)
        {
            if (obj is DBNull)
                return defVal;
            else
                return (decimal)obj;
        }
        protected static bool GetBoolean(object obj, bool defVal = false)
        {
            if (obj is DBNull)
                return defVal;
            else
                return (bool)obj;
        }
        protected static DateTime GetDateTime(object obj, int defVal = 0)
        {
            if (obj is DBNull)
            {
                if (defVal == 0) return DateTime.MaxValue;
                else if (defVal == 1) return DateTime.MinValue;
                else return DateTime.Now;
            }
            else
                return (DateTime)obj;
        }
        protected static string GetString(object obj)
        {
            return obj + "";
        }

        #endregion 
    }
	
    public class PageingModel<T>
    {
        // 输入属性
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }

        // 输出属性
        public int PageCount{get;set;}
        public int RecordCount{get;set;}
        public IList<T> List{get;set;}
    }
	
    public class JsonResult<T>
    {
        public string status{get;set;}
        
        public string message{get;set;}
        
        public T data{get;set;}
       
    }
    
    public interface ICondition
    {
         string ToConditionString();
         List<SqlParameter> ToParam();
    }
    
    public class SEARCH_CONDITION : ICondition
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string OS { get; set; }
        public string ToConditionString()
        {
            if (Name == null || Value == null || OS == null )
                return " ( 1=1 ) ";
            else
                return " ( " + Name + " " + OS + " " + " @" + Name + " ) ";
        }
        public List<SqlParameter> ToParam()
        {
            List<SqlParameter> Parms = new List<SqlParameter>();
            if (Name == null || Value == null || OS == null)
            {
                int i = 1;
            }
            else {
                Parms.Add(new SqlParameter(Name, Value));
            }
            
            return Parms;
        }
    }
    
    public class SEARCH_CONDITION_GROUP : ICondition
    {
        public ICondition ConditionA { get; set; }
        public ICondition ConditionB { get; set; }
        public string OperatorSign { get; set; }
        public string ToConditionString()
        {
            string sqla = " (1=1) ";
            string sqlb = " (1=1) ";
            string sign = " and ";
            if (ConditionA != null) sqla = ConditionA.ToConditionString();
            if (ConditionB != null) sqlb = ConditionB.ToConditionString();
            if (OperatorSign != null) sign = OperatorSign;
                
            return " ( "+sqla + sign + sqlb+" ) ";
        }

        public List<SqlParameter> ToParam()
        {
            List<SqlParameter> Parms = new List<SqlParameter>();
            Parms.AddRange(ConditionA.ToParam());
            Parms.AddRange(ConditionB.ToParam());
            return Parms;
        }
    }
}

							
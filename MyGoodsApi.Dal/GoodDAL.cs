//============================================================
// Producnt name:		TenXiang.Code
// Coded by:			hotboart
// Auto generated at: 	2020/5/5 星期二 16:01:54
//============================================================



using System;						
using System.Collections.Generic;	
using System.Text;					
using System.Data;					
using System.Data.SqlClient;		
using Net.Itcast.CN.Model;



namespace Net.Itcast.CN.DAL
{								

	public partial class GoodDAL : DBHelper<Good>
	{	
        /// <summary>
        /// 创建实体对象
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="SqlDataReader">数据读取器</param>
		/// <returns>实体</returns>		
        protected override Good CreateSingle(SqlDataReader reader)
        {
                    Good temp = new Good();
					
					  temp.Id	=GetInt(reader["id"]);
					  temp.Name	=GetString(reader["name"]);
					  temp.Price	=GetDecimal(reader["price"]);
					  temp.Productaddr	=GetString(reader["productAddr"]);
					  temp.Remark	=GetString(reader["remark"]);
					  temp.Typeid	=GetInt(reader["typeid"]);
					  temp.Type	=new GoodstypeDAL().GetGoodstypeById(temp.Typeid);
                    return temp;
        }
		/// <summary>
        /// 添加方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Good">实体对象</param>
		/// <returns>新实体ID</returns>		
	    public  int AddGood(Good good)
	    {	
			string sql = "INSERT Goods (name, price, productAddr, remark, typeid)VALUES (@name, @price, @productAddr, @remark, @typeid);SELECT @@identity;";							
            SqlParameter[] para = new SqlParameter[]
           						  {
										new SqlParameter("@name",good.Name), new SqlParameter("@price",good.Price), new SqlParameter("@productAddr",good.Productaddr), new SqlParameter("@remark",good.Remark), new SqlParameter("@typeid",good.Typeid)
								  }	;									
    	    return ExecNonQuery(sql, para);
		}  
        /// <summary>
        /// 修改方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Good">实体对象</param>
		/// <returns>受影响行数</returns>		
		public  int UpdateGood(Good good)
	    {	
			string sql = "Update Goods set name=@name, price=@price, productAddr=@productAddr, remark=@remark, typeid=@typeid where id=@id";
            SqlParameter[] para = new SqlParameter[]
           						  {
										new SqlParameter("@id",good.Id), new SqlParameter("@name",good.Name), new SqlParameter("@price",good.Price), new SqlParameter("@productAddr",good.Productaddr), new SqlParameter("@remark",good.Remark), new SqlParameter("@typeid",good.Typeid)
								  }	;									
    	    return ExecNonQuery(sql, para);
		}  
        /// <summary>
        /// 删除方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Id">实体对象主键</param>
		/// <returns>受影响行数</returns>	
		public  int DelGood(int Id)
	    {		
			string sql = "Delete FROM Goods  where id=@id";
            SqlParameter[] para = new SqlParameter[]
           						  {
										new SqlParameter("@id",Id)
								  }	;									
    	    return ExecNonQuery(sql, para);
		}
        
        /// <summary>
        /// 删除方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Id">实体对象主键组 如：“1,2,3,4”</param>
		/// <returns>受影响行数</returns>	
		public  int DelsGood(string Ids)
	    {		
			string sql = "Delete FROM Goods  where id in (@id)";
            SqlParameter[] para = new SqlParameter[]
           						  {
										new SqlParameter("@id",Ids)
								  }	;									
    	    return ExecNonQuery(sql, para);
		}
        
        /// <summary>
        /// 根据主键查询对象
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Id">实体对象主键</param>
		/// <returns>实体对象</returns>
		public  Good GetGoodById(int Id)  
		{	
			string sql = "Select * FROM Goods where id =@id";							
			SqlParameter para = new SqlParameter("@id",Id);
    	    return GetOne(sql,para);
		}
        /// <summary>
        /// 查询全部对象
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name=""></param>
		/// <returns>结果集</returns>
		public  IList< Good > GetAllList()
		{	
			string sql = "Select * FROM Goods";
    	    return GetList(sql);
		}
	
		/// <summary>
        /// 根据条件分页查询
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="">分页对象</param>
        /// <param name="">查询条件</param>
		/// <returns>结果集</returns>
		public  void  GetPagingList(PageingModel<Good> Paging,ICondition contion)
		{	
			string Sql = "Select * FROM Goods"+" where " + contion.ToConditionString();
            SqlParameter [] para = contion.ToParam().ToArray();
            GetPaging(Paging,Sql,para);
		}
        
        
	}
}
		

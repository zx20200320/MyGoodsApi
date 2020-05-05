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

	public partial class GoodstypeDAL : DBHelper<Goodstype>
	{	
        /// <summary>
        /// 创建实体对象
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="SqlDataReader">数据读取器</param>
		/// <returns>实体</returns>		
        protected override Goodstype CreateSingle(SqlDataReader reader)
        {
                    Goodstype temp = new Goodstype();
					
					  temp.Id	=GetInt(reader["id"]);
					  temp.Name	=GetString(reader["name"]);
                    return temp;
        }
		/// <summary>
        /// 添加方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Goodstype">实体对象</param>
		/// <returns>新实体ID</returns>		
	    public  int AddGoodstype(Goodstype goodstype)
	    {	
			string sql = "INSERT GoodsType (name)VALUES (@name);SELECT @@identity;";							
            SqlParameter[] para = new SqlParameter[]
           						  {
										new SqlParameter("@name",goodstype.Name)
								  }	;									
    	    return ExecNonQuery(sql, para);
		}  
        /// <summary>
        /// 修改方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Goodstype">实体对象</param>
		/// <returns>受影响行数</returns>		
		public  int UpdateGoodstype(Goodstype goodstype)
	    {	
			string sql = "Update GoodsType set name=@name where id=@id";
            SqlParameter[] para = new SqlParameter[]
           						  {
										new SqlParameter("@id",goodstype.Id), new SqlParameter("@name",goodstype.Name)
								  }	;									
    	    return ExecNonQuery(sql, para);
		}  
        /// <summary>
        /// 删除方法
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="Id">实体对象主键</param>
		/// <returns>受影响行数</returns>	
		public  int DelGoodstype(int Id)
	    {		
			string sql = "Delete FROM GoodsType  where id=@id";
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
		public  int DelsGoodstype(string Ids)
	    {		
			string sql = "Delete FROM GoodsType  where id in (@id)";
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
		public  Goodstype GetGoodstypeById(int Id)  
		{	
			string sql = "Select * FROM GoodsType where id =@id";							
			SqlParameter para = new SqlParameter("@id",Id);
    	    return GetOne(sql,para);
		}
        /// <summary>
        /// 查询全部对象
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name=""></param>
		/// <returns>结果集</returns>
		public  IList< Goodstype > GetAllList()
		{	
			string sql = "Select * FROM GoodsType";
    	    return GetList(sql);
		}
	
		/// <summary>
        /// 根据条件分页查询
        /// 作者：HOTBOART 时间：2020/5/5 星期二
		/// </summary>
		/// <param name="">分页对象</param>
        /// <param name="">查询条件</param>
		/// <returns>结果集</returns>
		public  void  GetPagingList(PageingModel<Goodstype> Paging,ICondition contion)
		{	
			string Sql = "Select * FROM GoodsType"+" where " + contion.ToConditionString();
            SqlParameter [] para = contion.ToParam().ToArray();
            GetPaging(Paging,Sql,para);
		}
        
        
	}
}
		

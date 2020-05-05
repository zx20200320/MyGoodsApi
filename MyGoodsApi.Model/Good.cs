//============================================================
// Producnt name:		TenXiang.Code
// Coded by:			hotboart
// Auto generated at: 	2020/5/5 星期二 16:01:54
//============================================================


using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Itcast.CN.Model
{
	
	public class Good
	{							
		public Good(){}  

        public int Id{get;set;}
		
		private int typeid;			
		public int Typeid
		{
			get{return typeid;}
			set{typeid=value;}
		}		
		private Goodstype type;
		public Goodstype Type
		{
			get{return type;}
			set{type=value;}
		}		
		public  string Name{get;set;}
		public  decimal Price{get;set;}
		public  string Productaddr{get;set;}
		public  string Remark{get;set;}
	}							
}								
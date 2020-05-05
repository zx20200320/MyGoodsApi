


//============================================================
// Producnt name:		TenXiang.Code
// Coded by:			hotboart
// Auto generated at: 	2020/5/5 星期二 16:01:54
//============================================================

using System;
using System.Collections.Generic;
using System.Text;
using Net.Itcast.CN.DAL;
using Net.Itcast.CN.Model;

namespace Net.Itcast.CN.BLL
{								
	public static partial class GoodstypeBLL
	{	
		
        private static GoodstypeDAL service = new GoodstypeDAL();
        
	    public static int AddGoodstype(Goodstype goodstype)
	    {								
			return service.AddGoodstype(goodstype);
		}  
		public static int UpdateGoodstype(Goodstype goodstype)
	    {								
			return service.UpdateGoodstype(goodstype);
		}  
		public static int DelGoodstype(int Id)
	    {								
			return service.DelGoodstype(Id);
		}
        public static int DelsGoodstype(string Ids)
	    {								
			return service.DelsGoodstype(Ids);
		}
		public static IList< Goodstype > GetAllList()
		{								
			return service.GetAllList();
		}		
		public static Goodstype GetGoodstypeById(int Id)  
		{								
			return service.GetGoodstypeById(Id);
		}
        public static void GetPagingList(PageingModel<Goodstype> Paging, ICondition contion)
        {
            service.GetPagingList(Paging, contion);
        }
	}
}
		



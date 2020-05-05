


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
	public static partial class GoodBLL
	{	
		
        private static GoodDAL service = new GoodDAL();
        
	    public static int AddGood(Good good)
	    {								
			return service.AddGood(good);
		}  
		public static int UpdateGood(Good good)
	    {								
			return service.UpdateGood(good);
		}  
		public static int DelGood(int Id)
	    {								
			return service.DelGood(Id);
		}
        public static int DelsGood(string Ids)
	    {								
			return service.DelsGood(Ids);
		}
		public static IList< Good > GetAllList()
		{								
			return service.GetAllList();
		}		
		public static Good GetGoodById(int Id)  
		{								
			return service.GetGoodById(Id);
		}
        public static void GetPagingList(PageingModel<Good> Paging, ICondition contion)
        {
            service.GetPagingList(Paging, contion);
        }
	}
}
		



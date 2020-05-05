using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Net.Itcast.CN.BLL;
using Net.Itcast.CN.Model;
using Net.Itcast.CN.DAL;
using MyGoodsApi.Models;

namespace MyGoodsApi.Controllers
{
    public class DefaultController : ApiController
    {
        
        public PageingModel<Good> GetPageing(int PageIndex,int PageSize)
        {
            PageingModel<Good> data = new PageingModel<Good>();
            data.PageIndex = PageIndex;
            data.PageSize = PageSize;
            data.Order = "Id";
            
            GoodBLL.GetPagingList(data,new SEARCH_CONDITION());
            return data;
        }
        [HttpGet]
        public void ExportFile()
        {
            APIFileHelp helper = new APIFileHelp();
            var list = GoodBLL.GetAllList().ToList();
            //我们在做导出，，所以，这里应该是字段在前，标题在后
            var dic = new Dictionary<string, string>() {
                { "Id" ,"编号"},
                { "Typeid" ,"类型编号"},
                { "Name" ,"名称"},
                { "Price" ,"单价"},
                { "Productaddr" ,"产地"},
                { "Remark" ,"备注"}
            };
            helper.ExportExcel<Good>("a.xls",list,dic);
        }
    }
}

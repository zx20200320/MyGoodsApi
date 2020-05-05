using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using System.Reflection;
using NPOI.SS.Util;
using NPOI.SS.UserModel;

namespace MyGoodsApi.Models
{
    /// <summary>
    /// 文件上传下载，导入导出辅助类
    /// </summary>
    public class APIFileHelp
    {
        public string[] ExtentsfileName = new string[] { ".doc", ".xls", ".png", ".jpg" };
        public string UrlPath = "/Upload/";
        /// <summary>
        ///响应对象 ，使用前先赋值
        /// </summary>
        public HttpResponse Response = HttpContext.Current.Response;
        public HttpRequest Request = HttpContext.Current.Request;
        /// <summary>
        /// 下载文件 使用过DEMO
        /// using System.IO;
        /// public class FileOperationController : ApiController
        /// {
        ///    APIFileHelp help = new APIFileHelp();
        ///    [HttpGet]
        ///    public void DownLoad(string Url)
        ///    {
        ///        string filePath = HttpContext.Current.Server.MapPath(Url);
        ///        FileInfo fi = new FileInfo(filePath);
        ///        help.DownLoad(fi.Name, fi.FullName);
        ///    }
        ///}
        ///
        /// <a href = "https://localhost:44370/api/FileOperation/DownLoad?Url=/FileUpload/132211303318715030.xls" > 下载 </ a >
        /// </summary>
        /// <param name="downFileName">下载后保存名</param>
        /// <param name="sourceFileName">服务器端物理路径</param>
        public void DownLoad(string downFileName, string sourceFileName)
        {
            if (File.Exists(sourceFileName))
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; FileName={0}", downFileName));
                Response.Charset = "GB2312";
                Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                Response.ContentType = MimeMapping.GetMimeMapping(downFileName);
                Response.WriteFile(sourceFileName);
                Response.Flush();
                Response.Close();
            }
        }
        /// <summary>
        /// 上传文件 控制器端
        /// public class FileOperationController : ApiController
        ///{
        ///    [HttpPost]
        ///    public FileResult UpLoad()
        ///    {
        ///        return help.UpLoad();
        ///    }
        ///}
        ///
        /// 
        /// 上传文件 客户端
        ///<input type = "file" id="f1" />
        ////<input type = "button" value="aa" onclick="ff()"/>

        ///< script >
        ///    function ff()
        ///{
        ///    var formData = new FormData();
        ///    var file = document.getElementById("f1").files[0];
        ///    formData.append("fileInfo", file);
        ///        $.ajax({
        ///    url: "https://localhost:44370/api/FileOperation/UpLoad",
        ///            type: "POST",
        ///            data: formData,
        ///            contentType: false,//必须false才会自动加上正确的Content-Type
        ///            processData: false,//必须false才会避开jQuery对 formdata 的默认处理，XMLHttpRequest会对 formdata 进行正确的处理
        ///            success: function(data) {
        ///            if (data.Code < 0)
        ///                alert(data.Msg)
        ///                else
        ///                alert(data.Url)
        ///            },
        ///            error: function(data) {
        ///            alert("上传失败！");
        ///        }
        ///    });
        ///}
        ///</script>
        ///
        /// </summary>
        /// <returns></returns>
        public FileResult UpLoad()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var extenfilename = Path.GetExtension(file.FileName);
                //判断 路径是否存在
                string path = HttpContext.Current.Server.MapPath(UrlPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (ExtentsfileName.Contains(extenfilename.ToLower()))
                {
                    string urlfile = UrlPath + DateTime.Now.ToFileTime() + extenfilename;
                    string filepath = HttpContext.Current.Server.MapPath(urlfile);
                    file.SaveAs(filepath);
                    return new FileResult() { Code = 0, Msg = "上传成功", Url = urlfile };
                }
                else
                {
                    return new FileResult() { Code = -1, Msg = "只允许上传指定格式文件" + string.Join(",", ExtentsfileName), Url = "" };
                }
            }
            else
            {
                return new FileResult() { Code = -1, Msg = "不能上传空文件", Url = "" };
            }
        }
        /// <summary>
        /// 导入
        /// 
        ///        public int Import()
        ///        {
        ///            int i = 0;
        ///            Dictionary<string, string> dic = new Dictionary<string, string>();
        ///             //                编号 姓名  性别 电话  照片 入学时间    毕业时间 班级ID    班级
        ///            dic.Add("编号", "Id");
        ///            dic.Add("姓名", "Name");
        ///            dic.Add("性别", "Sex");
        ///            dic.Add("电话", "Tel");
        ///            dic.Add("照片", "Photo");
        ///            dic.Add("入学时间", "JoinTime");
        ///            dic.Add("毕业时间", "OutTime");
        ///            dic.Add("班级ID", "CId");
        ///            dic.Add("班级", "CName");
        ///            // 导入上传文件的数据到集合中
        ///            List<Student> stus = help.ImportExcel<Student>(dic);

        ///            //把集合中的数据保存到数据库  循环添加，使用了事务
        ///            using (TransactionScope scope = new TransactionScope())
        ///            {
        ///                foreach (Student s in stus)
        ///                { 
        ///                    i++;
        ///                    dal.AddStudent(s);
        ///                }
        ///                scope.Complete();
        ///            }
        ///            return i;
        ///        }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic">字典集合 EXECL标题=》对象属性</param>
        /// <returns>对象集合</returns>
        public List<T> ImportExcel<T>(Dictionary<string, string> dic) where T : class, new()
        {

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                List<T> list = new List<T>();
                Type tp = typeof(T);

                using (var fs = file.InputStream)
                {
                    //把xls文件中的数据写入workbook1中
                    var workbook1 = new HSSFWorkbook(fs);
                    //获取第一个Sheet
                    var sheet = workbook1.GetSheetAt(0);
                    //获取第一行 标题行
                    var row = sheet.GetRow(0);
                    //声明字段数组
                    string[] fields = new string[row.LastCellNum];

                    for (var i = 0; i < row.LastCellNum; i++)
                    {

                        string title = row.GetCell(i).ToString();
                        if (dic.ContainsKey(title))
                        {
                            fields[i] = dic[title];
                        }
                    }
                    for (var j = 1; j <= sheet.LastRowNum; j++)
                    {
                        //读取当前行数据
                        var dataRow = sheet.GetRow(j);
                        // 创建对象实例
                        T t = new T();
                        if (dataRow != null)
                        {
                            for (var k = 0; k <= dataRow.LastCellNum; k++)
                            {   //当前表格 当前单元格 的值
                                var cell = dataRow.GetCell(k);
                                if (cell != null)
                                {
                                    var p = tp.GetProperty(fields[k]);
                                    if(p!=null)
                                    { 
                                        p.SetValue(t, GetValue(cell.ToString(), p));
                                    }
                                }
                            }
                        }
                        list.Add(t);
                    }
                }
                return list;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 导出
        /// [HttpGet]
        /// public void Export1(int PageSize, int PageIndex, int Cid)
        /// {
        ///    var list = dal.GetStudentByPaging(PageSize, PageIndex, Cid).Data;
        ///    Dictionary<string, string> dic = new Dictionary<string, string>();
        ///    dic.Add("Id", "编号");
        ///    dic.Add("Name", "姓名");
        ///    dic.Add("Sex", "性别");
        ///    dic.Add("Tel", "电话");
        ///    dic.Add("Photo", "照片");
        ///    dic.Add("JoinTime", "入学时间");
        ///    dic.Add("OutTime", "毕业时间");
        ///    dic.Add("CId", "班级ID");
        ///    dic.Add("CName", "班级");
        ///    help.ExportExcel<Student>("a.xls", list, dic);
        ///}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">保存到客户端的文件名</param>
        /// <param name="list">要导出的数据集合</param>
        /// <param name="dic">字典集合  属性=》标题</param>
        public void ExportExcel<T>(string fileName, List<T> list, Dictionary<string, string> dic) where T : class, new()
        {
            Type tp = typeof(T); //获取类型
            var ps = tp.GetProperties(); //获取属性集合

            //创建工作薄
            var workbook = new HSSFWorkbook();
            //创建表
            var table = workbook.CreateSheet("sheet1");
            //创建表头
            var mrow = table.CreateRow(0);
            mrow.CreateCell(0);
            mrow.CreateCell(1);
            mrow.CreateCell(2);
            mrow.CreateCell(3);

            ICellStyle cellstyle = workbook.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;

            mrow.Cells[0].CellStyle = cellstyle;


            table.AddMergedRegion(new CellRangeAddress(0,0,0, 3));
            mrow.Cells[0].SetCellValue("aaa");

            var row = table.CreateRow(1);
            for (int i = 0; i < ps.Length; i++)
            {
                if (dic.ContainsKey(ps[i].Name))
                {
                    var cell = row.CreateCell(i);//创建单元格
                    cell.SetCellValue(dic[ps[i].Name]);
                }
            }

            //模拟20行20列数据
            for (var i = 2; i < list.Count+1; i++)
            {
                //创建新行
                var dataRow = table.CreateRow(i);
                for (int j = 0; j < ps.Length; j++)
                {
                    if (dic.ContainsKey(ps[j].Name))
                    {
                        var cell = dataRow.CreateCell(j);//创建单元格
                        cell.SetCellValue(ps[j].GetValue(list[i-1]).ToString());
                    }
                }
            }

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; FileName={0}", fileName));
            Response.Charset = "GB2312";
            Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Response.ContentType = MimeMapping.GetMimeMapping(fileName);
            workbook.Write(Response.OutputStream);
            Response.Flush();
            Response.Close();
        }
        private static object GetValue(string obj, PropertyInfo p)
        {
            object o = null;
            switch (p.PropertyType.Name)
            {
                case "Int16":
                    o = Int16.Parse(obj);
                    break;
                case "Int32":
                    o = Int32.Parse(obj);
                    break;
                case "Int64":
                    o = Int64.Parse(obj);
                    break;
                case "double":
                    o = double.Parse(obj);
                    break;
                case "float":
                    o = float.Parse(obj);
                    break;
                case "String":
                    o = obj.ToString();
                    break;
                case "bool":
                    o = bool.Parse(obj);
                    break;
                case "DateTime":
                    o = DateTime.Parse(obj);
                    break;
            }
            return o;
        }
    }

    public class FileResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public string Url { get; set; }
    }


}



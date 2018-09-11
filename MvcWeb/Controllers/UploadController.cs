using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcWeb.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        #region 配置信息
        /// <summary>
        /// 资源站点URl
        /// </summary>
        public static string resourceSiteUrl = ConfigurationManager.AppSettings["ResourceSiteUrl"].ToString();
        /// <summary>
        /// 资源站点图片上传地址
        /// </summary>
        public static string resourceSitePostUrl = ConfigurationManager.AppSettings["ResourceSitePostUrl"].ToString();

        /// <summary>
        /// 上传非图片文件请求url,比如http://img.cai.com
        /// </summary>
        public static string UploadFileRequestUrl = ConfigurationManager.AppSettings["UploadFileRequestUrl"].ToString();
        /// <summary>
        /// 文件上传请求action，比如/UpLoad/UploadFile
        /// </summary>
        public static string UploadFileAction = ConfigurationManager.AppSettings["UploadFileAction"].ToString();
        /// <summary>
        /// 非图片文件保存目录，比如/Upload/files/
        /// </summary>
        public static string FileSavePath = ConfigurationManager.AppSettings["FileSavePath"].ToString();
        /// <summary>
        /// 判断是那个项目文件目录
        /// </summary>
        public static readonly string WebSiteEName = ConfigurationManager.AppSettings["WebSiteEName"].ToString();
        #endregion

        //[HttpPost]
        //public ContentResult UpLoadImage()
        //{
        //    try
        //    {
        //        var file = Request.Files["imgFile"];
        //        string nameImg = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //        string upLoadFile = ConfigurationManager.AppSettings["UpLoadFile"].ToString();
        //        string upLoadPostPath = ConfigurationManager.AppSettings["UpLoadPostPath"].ToString();
        //        nameImg += file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
        //        string url = string.Format("{0}{1}{2}", resourceSiteUrl, upLoadFile, nameImg);

        //        upLoadFile = "/" + ConfigurationManager.AppSettings["WebSiteEName"].ToString() + upLoadFile;

        //        string postUrl = string.Format("{0}{1}?filename={2}&upLoadFile={3}", resourceSitePostUrl, upLoadPostPath, nameImg, upLoadFile);

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
        //        request.Method = "POST";
        //        request.AllowAutoRedirect = false;
        //        request.ContentType = "multipart/form-data";
        //        byte[] bytes = new byte[file.InputStream.Length];
        //        file.InputStream.Read(bytes, 0, (int)file.InputStream.Length);
        //        request.ContentLength = bytes.Length;
        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(bytes, 0, bytes.Length);
        //        }
        //        HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
        //        Hashtable hash = new Hashtable();
        //        hash["error"] = 0;
        //        hash["url"] = url;
        //        return Content(System.Web.Helpers.Json.Encode(hash), "text/html; charset=UTF-8");

        //    }
        //    catch (Exception ex)
        //    {
        //        var writer = Common.Log.LogWriterGetter.GetLogWriter();
        //        writer.Write("UploadFiles", "-Admin_Upload", ex);
        //        throw ex;
        //    }
        //}

        //public string UpLoadFile(string fromFilePath, string toFilePath, string fileName)
        //{
        //    fromFilePath = "/Xml/test.xml";
        //    toFilePath = "/UpLoad/uploadimages/";
        //    try
        //    {
        //        FileInfo fi = new FileInfo(fromFilePath);
        //        FileStream fs = fi.OpenRead();
        //        string resourceSiteUrl = ConfigurationManager.AppSettings["ResourceSiteUrl"].ToString();

        //        string postUrl = resourceSiteUrl + "/UpLoad/upload_json.aspx" + "?filename=" + fileName + "&upLoadFile=" + toFilePath;

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
        //        request.Method = "POST";
        //        request.AllowAutoRedirect = false;
        //        request.ContentType = "multipart/form-data";
        //        byte[] bytes = new byte[fs.Length];
        //        fs.Read(bytes, 0, (int)fs.Length);

        //        request.ContentLength = bytes.Length;
        //        using (Stream requestStream = request.GetRequestStream())
        //        {
        //            requestStream.Write(bytes, 0, bytes.Length);
        //        }
        //        HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
        //        Hashtable hash = new Hashtable();
        //        hash["error"] = 0;
        //        hash["url"] = toFilePath + fileName;

        //        //return Content(System.Web.Helpers.Json.Encode(hash), "text/html; charset=UTF-8");
        //        return "";

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UploadFileNew()
        {
            Hashtable hash = new Hashtable();
            hash["code"] = 0;
            try
            {
                var file = Request.Files["uploadFile"];
                if (file == null)
                    throw new Exception("file is null");

                string newFileName = DateTime.Now.ToString("yyMMddHHmmssfff");
                newFileName += file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();

                string SavePath = string.Format("/{0}/{1}/", WebSiteEName, FileSavePath); //比如：/HtAdmin/Upload/files/
                string fullPath = string.Format("{0}{1}{2}", UploadFileRequestUrl, SavePath, newFileName);//比如：http:img.cai.com/HtAdmin/Upload/files/newFileName

                //上传接口
                string postUrl = string.Format("{0}/{1}?filename={2}&savepath={3}", UploadFileRequestUrl, UploadFileAction, newFileName, SavePath);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
                request.Method = "POST";
                request.AllowAutoRedirect = false;
                request.ContentType = "multipart/form-data";
                byte[] bytes = new byte[file.InputStream.Length];
                file.InputStream.Read(bytes, 0, (int)file.InputStream.Length);
                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse respon = (HttpWebResponse)request.GetResponse();
                if (respon.StatusCode == HttpStatusCode.OK)
                {
                    hash["name"] = newFileName;
                    hash["fullpath"] = fullPath;
                }
                else
                {
                    hash["code"] = -1;
                    hash["msg"] = "error";
                }
            }
            catch (Exception ex)
            {
                hash["code"] = -1;
                hash["msg"] = ex.Message;
            }
            return Content(System.Web.Helpers.Json.Encode(hash), "text/html; charset=UTF-8");
        }
    }
}
﻿using System;
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

        /// <summary>
        /// 资源站点URl
        /// </summary>
        private static string resourceSiteUrl = ConfigurationManager.AppSettings["ResourceSiteUrl"].ToString();
        /// <summary>
        /// 资源站点图片上传地址
        /// </summary>
        private static string resourceSitePostUrl = ConfigurationManager.AppSettings["ResourceSitePostUrl"].ToString();

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
        /// 文件保存目录，比如/Upload/files/
        /// </summary>
        private static string FileSavePath = ConfigurationManager.AppSettings["FileSavePath"].ToString();
        /// <summary>
        /// 文件上传请求url，比如/UpLoad/UploadFile
        /// </summary>
        private static string UploadFilePostUrl = ConfigurationManager.AppSettings["UploadFilePostUrl"].ToString();
        /// <summary>
        /// 判断是那个项目文件目录
        /// </summary>
        private static readonly string WebSiteName = ConfigurationManager.AppSettings["WebSiteName"].ToString();
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //[HttpPost]
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

                string SavePath = "/" + WebSiteName + FileSavePath; //比如：/HtAdmin/Upload/files/
                string fullPath = string.Format("{0}{1}{2}", resourceSiteUrl, SavePath, newFileName);//比如：http:img.cai.com/HtAdmin/Upload/files/newFileName

                //上传接口,在资源站点/UpLoad/UploadFile
                string postUrl = string.Format("{0}{1}?filename={2}&savepath={3}", resourceSiteUrl, UploadFilePostUrl, newFileName, SavePath);

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

                hash["name"] = newFileName;
                hash["msg"] = fullPath;
            }
            catch (Exception ex)
            {
                hash["code"] = -1;
                hash["name"] = "";
                hash["msg"] = "error:" + ex.Message;
            }
            return Content(System.Web.Helpers.Json.Encode(hash), "text/html; charset=UTF-8");
        }
    }
}
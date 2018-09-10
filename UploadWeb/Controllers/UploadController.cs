using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadWeb.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult upload_json()
        {
            //return View();
            try
            {
                string filename = Request["filename"];
                string upLoadFile = Request["upLoadFile"];
                string path = AppDomain.CurrentDomain.BaseDirectory + upLoadFile;
                //if (Path.GetExtension(filename) != ".json")
                //    return Content("非法的文件");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (Stream stream = Request.InputStream)
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    System.IO.File.WriteAllBytes(path + filename, buffer);
                    Response.Write("上传成功" + filename);
                }
            }
            catch (Exception ex)
            {
                //var writer = Common.Log.LogWriterGetter.GetLogWriter();
                //writer.Write("UploadFiles", "-RES_Upload", ex);
                throw ex;
            }
            return View();
        }


        /// <summary>
        /// 上传文件 zwc.280906
        /// </summary>
        /// <returns></returns>
        public string UploadFile()
        {
            try
            {
                string filename = Request["filename"];
                string savePath = Request["savepath"];
                string path = AppDomain.CurrentDomain.BaseDirectory + savePath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (Stream stream = Request.InputStream)
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    System.IO.File.WriteAllBytes(path + filename, buffer);

                    return filename;
                }
            }
            catch
            {
                return "error";
            }
        }
    }
}
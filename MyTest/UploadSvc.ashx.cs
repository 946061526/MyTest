using MyTest.Excute;
using MyTest.WebReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyTest
{
    /// <summary>
    /// UploadSvc 的摘要说明
    /// </summary>
    public class UploadSvc : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write(UploadImage());
            UploadImg2();
        }
        private object UploadImage()
        {
            object returnobj = "ok";
            string who = GetResponse("who");
            HttpFileCollection files = HttpContext.Current.Request.Files;
            int count = files.Count;
            for (int i = 0; i < count; i++)
            {
                HttpPostedFile file = files[i];
                string tmpPath = HttpContext.Current.Server.MapPath("/Upload/Media/");
                string fileName = System.IO.Path.GetFileName(file.FileName);
                try
                {
                    string severlocalpath = tmpPath + fileName;
                    file.SaveAs(severlocalpath);
                    returnobj = "ok";
                }
                catch (Exception ex)
                {
                    returnobj = ex.Message;
                }
            }
            return returnobj;
        }


        private void UploadImg2()
        {
            string base64String = HttpContext.Current.Request["base64_string"].ToString();
            //string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpeg";
            //string filePath = "Test/" + DateTime.Now.ToString("yyyy-MM-dd");
            //upLoadFiles uf = new upLoadFiles();
            //byte[] oneChunk = Base64StringToImage(base64String);
            //string req = uf.upLoadOneChunk(oneChunk, fileName, filePath);
            ////调用webservice将byte数组传过去upLoadOneChunk(文件字节流,"唯一文件名","文件夹名称");
            ////返回带路径文件名称，失败则返回“error：异常信息”
            ////Bitmap bt = new Bitmap(Image.FromStream(MyStream));   
            ////  Image bt = Image.FromStream(MyStream,false);
            //using (MemoryStream m = new MemoryStream(oneChunk, 0, oneChunk.Length))
            //{
            //    System.Drawing.Image bt = System.Drawing.Image.FromStream(m);
            //    int imgWidth = bt.Width;
            //    int imgHeight = bt.Height;
            //    bt.Dispose();
            //    HttpContext.Current.Response.Write(req);
            //}

            //将base64String 转为 byte数组
            byte[] byteArray = Convert.FromBase64String(base64String);

            string saveFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpeg";
            string path = HttpContext.Current.Server.MapPath("TestImg/" + saveFileName);// + DateTime.Now.ToString("yyyy-MM-dd")
            //使用文件流读取byte数组中的数据
            Stream s = new FileStream(path, FileMode.Create);
            s.Write(byteArray, 0, byteArray.Length);
            s.Close();

            HttpContext.Current.Response.Write(path);
        }

        public string upLoadOneChunk(byte[] bytes, string fileName, string savepath)
        {
            try
            {
                string addressName = HttpContext.Current.Server.MapPath("files/") + savepath;
                FileHelper.CreateDir(addressName);
                string fullAddress = addressName + "/" + fileName;
                System.IO.File.WriteAllBytes(fullAddress, bytes);
                return ("files/" + savepath + "/" + fileName).ToLower();
            }
            catch (Exception e)
            {
                return "error:" + e.Message;
            }
        }
        //base64编码的文本 转为    图片
        private byte[] Base64StringToImage(string txtFileName)
        {
            byte[] _byte = null;
            try
            {
                //FileStream ifs = new FileStream(txtFileName, FileMode.Open, FileAccess.Read);
                //StreamReader sr = new StreamReader(ifs);
                //String inputStr = sr.ReadToEnd();
                _byte = Convert.FromBase64String(txtFileName);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Base64StringToImage 转换失败\nException：" + ex.Message);
            }
            return _byte;
        }

        /// <summary>
        /// 将stream转换成字节流
        /// </summary>
        /// <param name="filestream">文件stream</param>
        /// <returns></returns>
        private byte[] ConvertStreamToByteBuffer(Stream filestream)
        {
            try
            {
                byte[] bytes = new byte[filestream.Length];
                filestream.Read(bytes, 0, bytes.Length);
                filestream.Seek(0, SeekOrigin.Begin);
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        private object GetMedia()
        {
            //System.Collections.Generic.List<FileINfo> lit = new System.Collections.Generic.List<FileINfo>();
            //string tmpPath = HttpContext.Current.Server.MapPath("/Upload/Media/");
            //string imgtype = "*.bmp|*.jpg|*.gif|*.png";
            //string[] ImageType = imgtype.Split('|');
            //for (int i = 0; i < ImageType.Length; i++)
            //{
            //    string[] dirs = System.IO.Directory.GetFiles(tmpPath, ImageType[i]);
            //    int j = 0;
            //    foreach (string dir in dirs)
            //    {
            //        System.IO.FileInfo file = new System.IO.FileInfo(dir);
            //        FileINfo finfo = new FileINfo();
            //        finfo.FIlename = j.ToString();
            //        finfo.FileUrll = "../../Upload/Media/" + file.Name;
            //        lit.Add(finfo);
            //        j++;
            //    }
            //}
            //return lit;
            return null;
        }
        public string GetResponse(string responsevalue)
        {
            //判断提交方式
            HttpContext context = HttpContext.Current;
            string id = "";
            if (context.Request.RequestType.ToLower() == "get")
            {
                if (context.Request.QueryString[responsevalue] == null)
                {
                    return "";
                }
                id = context.Request.QueryString[responsevalue];
            }
            else
            {
                if (context.Request.Form[responsevalue] == null)
                {
                    return "";
                }
                id = context.Request.Form[responsevalue];
            }
            return id;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
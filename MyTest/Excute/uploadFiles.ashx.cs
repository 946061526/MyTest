using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MyTest.WebReference;

namespace MyTest.Excute
{
    /// <summary>
    /// uploadFiles 的摘要说明
    /// </summary>
    public class uploadFiles : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UploadFile(context);//执行上传方法
        }

        public void UploadFile(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";
            string s_rpath = FileHelper.GetUploadPath();//获取上传路径

            string Datedir = DateTime.Now.ToString("yy-MM-dd");
            string updir = s_rpath + "\\" + Datedir;
            string extname = string.Empty;
            string fullname = string.Empty;
            string filename = string.Empty;
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    if (!Directory.Exists(updir))//判断路径是否存在，不存在则创建
                    {
                        Directory.CreateDirectory(updir);
                    }
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        int offset = Convert.ToInt32(context.Request["chunk"]);
                        int total = Convert.ToInt32(context.Request["chunks"]);
                        string name = context.Request["name"];

                        //文件没有分块
                        if (total <= 1)
                        {
                            if (uploadFile.ContentLength > 0)
                            {
                                extname = Path.GetExtension(uploadFile.FileName);
                                fullname = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                                filename = uploadFile.FileName;

                                uploadFile.SaveAs(string.Format("{0}\\{1}", updir, filename));
                                context.Response.Write(string.Format("{0}\\{1}", updir, filename));
                            }
                        }
                        else
                        {
                            //文件 分成多块上传
                            fullname = WriteTempFile(uploadFile, offset);
                            if (total - offset == 1)
                            {
                                //如果是最后一个分块文件 ，则把文件从临时文件夹中移到上传文件 夹中
                                System.IO.FileInfo fi = new System.IO.FileInfo(fullname);
                                string oldFullName = string.Format("{0}\\{1}", updir, uploadFile.FileName);
                                FileInfo oldFi = new FileInfo(oldFullName);
                                if (oldFi.Exists)
                                {
                                    //文件名存在则删除旧文件 
                                    oldFi.Delete();
                                }
                                fi.MoveTo(oldFullName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("Message" + ex.ToString());
                }
            }
        }
        /// <summary>
        /// 保存临时文件 
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="chunk"></param>
        /// <returns></returns>
        private string WriteTempFile(HttpPostedFile uploadFile, int chunk)
        {

            string tempDir = FileHelper.GetTempPath();
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            string fullName = string.Format("{0}\\{1}.part", tempDir, uploadFile.FileName);
            if (chunk == 0)
            {
                //如果是第一个分块，则直接保存
                uploadFile.SaveAs(fullName);
            }
            else
            {
                //如果是其他分块文件 ，则原来的分块文件，读取流，然后文件最后写入相应的字节
                FileStream fs = new FileStream(fullName, FileMode.Append);
                if (uploadFile.ContentLength > 0)
                {
                    int FileLen = uploadFile.ContentLength;
                    byte[] input = new byte[FileLen];

                    // 初始化数据流
                    System.IO.Stream MyStream = uploadFile.InputStream;

                    // 读取文件进字节流
                    MyStream.Read(input, 0, FileLen);

                    fs.Write(input, 0, FileLen);
                    fs.Close();
                }
            }
            return fullName;
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
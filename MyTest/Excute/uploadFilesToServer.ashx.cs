using MyTest.WebReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyTest.Excute
{
    /// <summary>
    /// uploadFilesToServer 的摘要说明
    /// </summary>
    public class uploadFilesToServer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UploadFile(context);
        }
        public void UploadFile(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";
            string fileName =  "test/" + DateTime.Now.ToString("yyyy-MM-dd");
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    //upLoadFiles fHelper = new upLoadFiles();
                    FileHelper fHelper = new FileHelper();
                    
                    int filesCount = context.Request.Files.Count;
                    for (int j = 0; j < filesCount; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        int offset = Convert.ToInt32(context.Request["chunk"]);
                        int total = Convert.ToInt32(context.Request["chunks"]);
                        string name = context.Request["name"];//获取唯一的文件名
                        //文件没有分块
                        if (total == 1)
                        {
                            if (uploadFile.ContentLength > 0)
                            {
                                System.IO.Stream MyStream = uploadFile.InputStream;
                                byte[] oneChunk = ConvertStreamToByteBuffer(MyStream);
                                string req = fHelper.upLoadOneChunk(oneChunk, name, fileName);
                                //调用webservice将byte数组传过去upLoadOneChunk(文件字节流,"唯一文件名","文件夹名称");
                                //返回带路径文件名称，失败则返回“error：异常信息”
                                context.Response.Write(req);
                            }
                        }
                        else//文件分块上传
                        {
                            //文件 分成多块上传
                            //如果offest==0则表示是第一分块
                            if (offset == 0)
                            {
                                string req = fHelper.upLoadFirstChunk(ConvertStreamToByteBuffer(uploadFile.InputStream), name);
                                //调用webservice将byte数组传过去upLoadFirstChunk(文件字节流,"唯一文件名");
                                //返回成功或异常信息，成功返回字符串“success”，失败返回 “error：异常信息”
                                if (req.Contains("success"))
                                {
                                    context.Response.Write(req.Substring(0, 6));
                                }
                                else
                                {
                                    context.Response.Write(req);
                                }
                            }
                            else//如果不是第一分块
                            {
                                int fileLen = uploadFile.ContentLength;
                                string req = fHelper.upLoadOtherChunk(ConvertStreamToByteBuffer(uploadFile.InputStream), fileLen, name);
                                //调用webservice将byte数组传过去upLoadOtherChunk(文件字节流,文件长度,"唯一文件名");
                                //返回成功或异常信息，成功返回字符串“success”，失败返回 “error：异常信息”                               
                            }
                            //上传完毕将文件服务器临时文件转换成正式文件
                            if (total - offset == 1)
                            {
                                string req = fHelper.upLoadLastChunk(name, fileName);
                                //调用webservice将byte数组传过去upLoadLastChunk(唯一文件名,"文件夹名称");
                                //返回带路径文件名称，失败则返回“error：异常信息”  
                                if (req.Contains("success"))
                                {
                                    context.Response.Write(req.Substring(0, 6));
                                }
                                else
                                {
                                    context.Response.Write(req);
                                }
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}
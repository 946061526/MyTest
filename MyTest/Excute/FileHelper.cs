using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyTest.Excute
{
    /// <summary>
    ///FileHelper 的摘要说明
    /// </summary>
    public class FileHelper
    {
        public FileHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获取上传目录
        /// </summary>
        /// <returns></returns>
        public static string GetUploadPath()
        {
            string path = HttpContext.Current.Server.MapPath( "~/" );
            string dirname = GetDirName();
            string uploadDir = path + dirname;
            CreateDir( uploadDir );
            return uploadDir;
        }
        /// <summary>
        /// 获取临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            string path = HttpContext.Current.Server.MapPath( "~/" );
            string dirname = GetTempDirName();
            string uploadDir = path + dirname;
            CreateDir( uploadDir );
            return uploadDir;
        }
        private static string GetDirName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["uploaddir"];
            //获取配置文件该节点名称，用于上传文件存储文件夹
        }
        private static string GetTempDirName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["tempdir"];
            //获取配置文件该节点名称，用于上传文件临时文件夹
        }
        public static void CreateDir( string path )
        {
            if ( !System.IO.Directory.Exists( path ) )
            {
                System.IO.Directory.CreateDirectory( path );
            }
        }

        /// <summary>
        /// 只有一个包的时候直接存储
        /// </summary>
        /// <param name="Bytes">文件字节流</param>
        /// <param name="FileName">文件名</param>
        /// <param name="savepath">files下的路径</param>
        /// <returns>返回保存路径和文件名/异常信息</returns>
        public string upLoadOneChunk( byte[] bytes, string fileName, string savepath )
        {
            try
            {
                string addressName = HttpContext.Current.Server.MapPath( "files/" ) + savepath;
                FileHelper.CreateDir( addressName );
                string fullAddress = addressName + "/" + fileName;
                System.IO.File.WriteAllBytes( fullAddress, bytes );
                return ( "files/" + savepath + "/" + fileName ).ToLower();
            }
            catch ( Exception e )
            {
                return "error:" + e.Message;
            }
        }



        /// <summary>
        /// 分块上传时获得第一part部分，先创建文件第一部分part文件
        /// </summary>
        /// <param name="bytes">第一部分文件字节流</param>
        /// <param name="fileName">文件名</param>
        /// <returns>成功/异常信息</returns>
        public string upLoadFirstChunk( byte[] bytes, string fileName )
        {
            try
            {
                string addressName = HttpContext.Current.Server.MapPath( "temp/" );
                System.IO.File.WriteAllBytes( addressName + fileName + ".part", bytes );
                return "success";
            }
            catch ( Exception e )
            {
                return "error:" + e.Message;
            }
        }

        /// <summary>
        /// 分块传输剩余其它文件部分
        /// </summary>
        /// <param name="bytes">文件字节流</param>
        /// <param name="fileLen">文件长度</param>
        /// <param name="fileName">唯一文件名</param>
        /// <returns>成功/异常信息</returns>
        public string upLoadOtherChunk( byte[] bytes, int fileLen, string fileName )
        {
            try
            {
                byte[] input = new byte[fileLen];
                Stream stream = new MemoryStream( bytes );
                string addressName = HttpContext.Current.Server.MapPath( "temp/" );
                FileStream fs = new FileStream( addressName + fileName + ".part", FileMode.Append );
                stream.Read( input, 0, fileLen );
                fs.Write( input, 0, fileLen );
                fs.Close();
                return "success";
            }
            catch ( Exception e )
            {
                return "error:" + e.Message;
            }
        }

        /// <summary>
        /// 执行最后一块part后（执行完upLoadOtherChunk），执行该方法，将文件转移至正式文件夹目录下
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="savepath">files下新的保存路径</param>
        /// <returns>返回保存路径和文件名/异常信息</returns>        
        public string upLoadLastChunk( string fileName, string savepath )
        {
            try
            {
                //把文件从临时文件夹中移到上传文件 夹中
                string addressName = HttpContext.Current.Server.MapPath( "temp/" ) + fileName + ".part";
                System.IO.FileInfo fi = new System.IO.FileInfo( addressName );
                string newFullPath = HttpContext.Current.Server.MapPath( "files/" ) + savepath;
                FileHelper.CreateDir( newFullPath );
                string oldFullName = string.Format( "{0}\\{1}", newFullPath, fileName );
                FileInfo oldFi = new FileInfo( oldFullName );
                if ( oldFi.Exists )
                {
                    //文件名存在则删除旧文件 
                    oldFi.Delete();
                }
                fi.MoveTo( oldFullName );
                return ( "files/" + savepath + "/" + fileName ).ToLower();
            }
            catch ( Exception e )
            {
                return "error:" + e.Message;
            }
        }


        /// <summary>
        /// IOS端上传函数
        /// </summary>
        /// <param name="str_bytes">上传文件数据流字符串</param>
        /// <param name="fileName">文件名</param>
        /// <param name="savepath">文件路径</param>
        /// <returns>带文件名的文件路径</returns>
        public string upLoadOneChunkForIOS( string str_bytes, string fileName, string savepath )
        {
            try
            {
                byte[] arr = Convert.FromBase64String( str_bytes );
                string addressName = HttpContext.Current.Server.MapPath( "files/" ) + savepath;
                FileHelper.CreateDir( addressName );
                System.IO.File.WriteAllBytes( addressName + "/" + fileName, arr );
                return ( "files/" + savepath + "/" + fileName ).ToLower();
            }
            catch ( Exception e )
            {
                return "error:" + e.Message;
            }
        }
    }

    #region

    //     public HttpResponseMessage Upload()
    //        {
    //            // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
    //            var files = HttpContext.Current.Request.Files;

    //            //// do something with the file in this space 
    //            //// {....}
    //            //// end of file doing

    //            //// Now we need to wire up a response so that the calling script understands what happened
    //            //HttpContext.Current.Response.ContentType = "text/plain";
    //            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
    //            //var result = new { name = file.FileName };

    //            //HttpContext.Current.Response.Write(serializer.Serialize(result));
    //            //HttpContext.Current.Response.StatusCode = 200;

    //            // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
    //            for (int i = 0; i < files.Count; i++)
    //            {
    //                var file = files[i];
    //                var path = Path.Combine(HttpContext.Current.Request.MapPath("/Upload"), file.FileName);
    //                long fileLenth;
    //                var startPost = new UploadProcess().SaveAs(path, file,out fileLenth);

    //                HttpContext.Current.Response.Write(serializer.Serialize(new { name = file.FileName }));
    //                HttpContext.Current.Response.StatusCode = 200;
    //                HttpContext.Current.Response.AddHeader("Range", string.Format("bytes={0}-{1}/{2}",0, startPost,fileLenth));
    //            }
    //            return new HttpResponseMessage(HttpStatusCode.OK);
    //        }
    //public long SaveAs(string saveFilePath, HttpPostedFile file,out long fullLenth)
    //        {
    //            //本地文件长度
    //            long lStartPos = 0;
    //            //开始读取位置
    //            long startPosition = 0;
    //            //结束读取位置
    //            int endPosition = 0;
    //            fullLenth = 0;
    //            var contentRange = HttpContext.Current.Request.Headers["Content-Range"];
    //            //bytes 10000-19999/1157632           
    //            if (!string.IsNullOrEmpty(contentRange))
    //            {
    //                contentRange = contentRange.Replace("bytes", "").Trim();
    //                string length = contentRange.Substring(contentRange.IndexOf("/")+1);
    //                fullLenth = int.Parse(length);
    //                contentRange = contentRange.Substring(0, contentRange.IndexOf("/"));

    //                string[] ranges = contentRange.Split('-');
    //                startPosition = int.Parse(ranges[0]);
    //                endPosition = int.Parse(ranges[1]);
    //            }
    //            System.IO.FileStream fs=null;

    //            try
    //            {
    //                if (System.IO.File.Exists(saveFilePath))
    //                {
    //                    fs = System.IO.File.OpenWrite(saveFilePath);
    //                    lStartPos = fs.Length;

    //                }
    //                else
    //                {
    //                    fs = new System.IO.FileStream(saveFilePath, System.IO.FileMode.Create);
    //                    lStartPos = 0;
    //                }
    //                if (lStartPos > endPosition)
    //                {
    //                    fs.Close();
    //                    //返回当前文件大小，通知浏览器从此位置开始上传
    //                    return lStartPos;
    //                }
    //                else if (lStartPos < startPosition)
    //                {
    //                    lStartPos = startPosition;
    //                }
    //                else if (lStartPos > startPosition && lStartPos < endPosition)
    //                {
    //                    lStartPos = startPosition;
    //                }
    //                fs.Seek(lStartPos, System.IO.SeekOrigin.Current);
    //                byte[] nbytes = new byte[1024];
    //                int nReadSize = 0;
    //                nReadSize = file.InputStream.Read(nbytes, 0, nbytes.Length);
    //                while (nReadSize > 0)
    //                {
    //                    fs.Write(nbytes, 0, nReadSize);
    //                    nReadSize = file.InputStream.Read(nbytes, 0, nReadSize);
    //                }

    //                fs.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                if (fs!=null)
    //                {
    //                    fs.Close();
    //                }
    //            }

    //            return endPosition;
    //        }

    #endregion
}
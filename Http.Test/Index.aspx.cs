using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Http.Test
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtResult.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string url = this.txtUrl.Text.Trim();
            string header = this.txtHeader.Text.Trim();
            string data = this.txtData.Text.Trim();
            string method = this.rdPost.Checked ? "POST" : "GET";
            //string fileParam = this.txtFileParam.Text.Trim();
            //string filePath = this.txtFilePath.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                ClientScript.RegisterStartupScript(this.GetType(), " message", "<script language='javascript' >alert('请输入请求地址');</script>");
            }
            else
            {
                try
                {

                    if (method == "POST")
                    {
                        this.txtResult.Text = this.HttpPost(url, data, header, "", "");
                    }
                    else
                    {
                        this.txtResult.Text = this.HttpGet(url, data, header);
                        //HttpGetAsync( url, data, header );
                    }
                }
                catch (Exception ex)
                {
                    this.txtResult.Text = "异常：" + ex.StackTrace;
                }
            }
        }

        private string HttpPost(string url, string data, string header, string fileParam, string filePath)
        {
            DateTime time = DateTime.Now;
            string arg = null;
            string result;
            try
            {
                if (!string.IsNullOrEmpty(fileParam))
                {
                    data += fileParam;
                }
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                if (!string.IsNullOrEmpty(header))
                {
                    httpWebRequest.Headers = new WebHeaderCollection
                    {
                        header
                    };
                }
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                httpWebRequest.Timeout = 200000;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (File.Exists(filePath))
                    {
                        FileStream fileStream = new FileStream(filePath, FileMode.Open);
                        byte[] array = new byte[fileStream.Length];
                        fileStream.Read(array, 0, array.Length);
                        requestStream.Write(array, 0, array.Length);
                    }
                }
                requestStream.Close();
                arg = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd();

                double ts = DateTime.Now.Subtract(time).TotalMilliseconds;
                result = "耗时" + ts + "ms,发送成功：" + arg;
            }
            catch (Exception ex)
            {
                result = "发送失败：" + ex.Message;
            }
            return result;
        }

        private string HttpGet(string url, string data, string header)
        {
            DateTime time = DateTime.Now;
            string arg = null;
            string result;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                if (!string.IsNullOrEmpty(header))
                {
                    httpWebRequest.Headers = new WebHeaderCollection
                    {
                        header
                    };
                }
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                arg = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd();

                double ts = DateTime.Now.Subtract(time).TotalMilliseconds;
                result = "耗时" + ts + "ms,发送成功：" + arg;
            }
            catch (Exception ex)
            {
                result = "发送失败：\t" + ex.Message;
            }
            return result;
        }
    }
}
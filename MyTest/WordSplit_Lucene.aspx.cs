using Lucene.Net.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTest
{
    public partial class WordSplit_Lucene : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            string t1 = "";
            int i = 0;
            Analyzer analyzer = new Lucene.China.ChineseAnalyzer();
            StringReader sr = new StringReader(TextBox1.Text);
            TokenStream stream = analyzer.TokenStream(null, sr);

            long begin = System.DateTime.Now.Ticks;
            Token t = stream.Next();
            while (t != null)
            {
                t1 = t.ToString();   //显示格式： (关键词,0,2) ，需要处理
                t1 = t1.Replace("(", "");
                char[] separator = { ',' };
                t1 = t1.Split(separator)[0];

                sb.Append(i + ":" + t1 + "\r\n");
                t = stream.Next();
                i++;
            }
            TextBox2.Text = sb.ToString();
            long end = System.DateTime.Now.Ticks; //100毫微秒
            int time = (int)((end - begin) / 10000); //ms


            TextBox2.Text += "耗时" + (time) + "ms \r\n=================================\r\n";
        }
    }
}
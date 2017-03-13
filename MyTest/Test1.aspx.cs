using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.ComponentModel.DataAnnotations;

namespace MyTest
{
    public partial class Test1 : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            //demo1:

            //StringBuilder _SBList = new StringBuilder();
            //_SBList.AppendFormat( "\"count\":{0},\"listItems\":[", 1 );
            //for ( int i = 0; i < 100000; i++ )
            //{
            //    _SBList.AppendFormat( "{0}\"logTime\":\"{1}\",\"logPointNum\":\"{2}\",\"logDescript\":\"{3}\"{4},",
            //        "{",
            //        "adfasdf",
            //        1,
            //        "asdfasfasdfasdfdsf",
            //    "}"
            //    );
            //}
            //string ss = _SBList.ToString().TrimEnd( ',' );

            //demo2:

            //StringBuilder _SBList = new StringBuilder();
            //_SBList.AppendFormat( "\"count\":{0},\"listItems\":[", 1 );
            //for ( int i = 0; i < 100000; i++ )
            //{
            //    if ( i > 0 )
            //        _SBList.Append( "," );
            //    _SBList.AppendFormat( "{0}\"logTime\":\"{1}\",\"logPointNum\":\"{2}\",\"logDescript\":\"{3}\"{4}",
            //        "{",
            //        "adfasdf",
            //        1,
            //        "asdfasfasdfasdfdsf",
            //    "}"
            //    );
            //}
            //string ss = _SBList.ToString();

            //demo3:
            //DateTime t1 = DateTime.Now;
            //DateTime t2 = DateTime.Now;
            //TimeSpan ts = t2 - t1;

            //Response.Write( ts.TotalMilliseconds );

            //demo4:
            //string ss = null;
            //Response.Write(string.Format("test{0}123",ss));


            ////demo5:
            //int i = 100 * 1024 * 1024;
            //Response.Write( "i=" + i.ToString() );

            ////demo6:
            //string ss = "";
            //if ( string.IsNullOrEmpty( ss ) )
            //    Response.Write( "Empty" );
            //else
            //    Response.Write( "空字符串" );

            ////demo7
            //string name = "karroy";
            //int age = 20;
            //string str1=$"{name},{age}";

            ////timespan
            //DateTime time1 = DateTime.Now;
            //DateTime time2 = time1.AddSeconds( 300 );
            //TimeSpan ts = time1 - time2;
            //double sec = ts.TotalSeconds;
            //Response.Write( "sec" + sec.ToString() );
        }

        protected void Button1_Click( object sender, EventArgs e )
        {
            //demo5
            StaticTest.auth = auth.Value;
            Response.Write( "auth: " + StaticTest.GetAuth() );
        }

        static public void sort<T>( IList<T> sortArray, Func<T, T, bool> comparison )
        {
            bool swapped = true;
            do
            {
                swapped = false;
                for ( int i = 0; i < sortArray.Count - 1; i++ )
                {
                    if ( comparison( sortArray[i + 1], sortArray[i] ) )
                    {
                        T tmp = sortArray[i];
                        sortArray[i] = sortArray[i + 1];
                        sortArray[i + 1] = tmp;
                        swapped = true;
                    }
                }
            } while ( swapped );
        }
    }

    
    public partial class Product
    {

    }
    public class Test
    {
        
        public object ID
        {
            get;
            set;
        }
    }

}
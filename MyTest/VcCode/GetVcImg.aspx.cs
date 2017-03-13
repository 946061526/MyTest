using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyTest.VcCode.BLL;
using MyTest.VcCode.util;
using NSYGShop;

namespace MyTest.VcCode
{
    public partial class GetVcImg : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            try
            {
                string _VcCharCookieKey = UtilityFun.ReqStr( "vcCharCookieKey", "" );
                if ( _VcCharCookieKey != "" )
                {
                    _VcCharCookieKey = UtilityCryptography.AESDecrypt( _VcCharCookieKey ); //得到 _vcWxCodeChar12060600

                    int width = UtilityFun.ReqNum( "width", 178 );
                    int height = UtilityFun.ReqNum( "height", 136 );

                    string _VcChar = Session[_VcCharCookieKey].ToString();
                    if ( string.IsNullOrEmpty( _VcChar ) )
                    {
                        HttpContext.Current.Response.ContentType = "text/html";
                        HttpContext.Current.Response.Write( "error1." );
                        HttpContext.Current.Response.End();
                    }

                    MemoryStream ms;
                    List<Position> list;
                    Position _Selected;

                    Draw _Draw = new Draw();
                    //这里还需要向couchBase里存一次，存验证码具体的字符和位置
                    _Draw.DrawVc( width, height, false, _VcChar, out ms, out list, out _Selected );

                    if ( _Selected != null && _Selected.ChineseChar != "" )
                    {
                        new VcBLL().SetVcCodeAuth( _Selected );
                        HttpContext.Current.Response.ContentType = "image/png";
                        HttpContext.Current.Response.BinaryWrite( ms.ToArray() );
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        HttpContext.Current.Response.ContentType = "text/html";
                        HttpContext.Current.Response.Write( "error1." );
                        HttpContext.Current.Response.End();
                    }
                }
                else
                {
                    HttpContext.Current.Response.ContentType = "text/html";
                    HttpContext.Current.Response.Write( "error2." );
                    HttpContext.Current.Response.End();
                }
            }
            catch
            {
                HttpContext.Current.Response.ContentType = "text/html";
                HttpContext.Current.Response.Write( "error3." );
                HttpContext.Current.Response.End();
            }
        }
    }
}
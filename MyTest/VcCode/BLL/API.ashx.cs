using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MyTest.VcCode.util;
using NSYGShop;

namespace MyTest.VcCode.BLL
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    public class API : IHttpHandler
    {
        private HttpContext _context;
        public void ProcessRequest( HttpContext context )
        {
            _context = context;
            context.Response.ContentType = "text/javascript";
            context.Response.Write( GetResult() );
        }

        public string GetResult()
        {
            string _Result = "\"code\":-3";
            string _Action = UtilityFun.ReqStrSQL( "action", "" );
            //是否get方式请求
            bool _IsGet = true;
            //是否通过http请求
            bool _IsHttp = false;

            switch ( _Action )
            {
                #region 获取已请求的验证码次数

                case "getVcCodeReqTimes":
                    try
                    {
                        string _Username = UtilityFun.ReqStr( "username", "" ); // 15802794545
                        string _Key = UtilityCryptography.Encrypt( _Username, 2 );
                        if ( string.IsNullOrEmpty( _Key ) )
                        {
                            _Result = "\"state\":0"; //
                        }
                        else
                        {
                            //int _ReqTimes = RequestLimit.CheckVcCodeRequestTimes( _Key );
                            //_Result = GetJsonStr( _ReqTimes >= BLLVar.VcCodeLimitTimes - 1 ? 1 : 0 );
                        }
                    }
                    catch ( Exception )
                    {
                        _Result = "\"state\":-2";
                    }
                    break;

                #endregion

                #region 新版验证码,获取一个字符
                case "getVcChar":
                    #region
                    try
                    {
                        string _Key = UtilityFun.ReqStrSQL( "key", "" );
                        string _IsCheck = UtilityFun.ReqStrSQL( "ischeck", "" ); //这个参数在注册功能没用上，后续观望
                        if ( _Key != "" && ( UtilityFun.IsMobile( _Key ) || UtilityFun.IsEmail( _Key ) ) )
                        {
                            //存入couchbase
                            string couchBasekey = UtilityCryptography.Encrypt( _Key, 2 );
                            int reqTimes = 1;//RequestLimit.CheckVcCodeRequestTimes( couchBasekey );
                            if ( reqTimes >= 2 )
                            {
                                _Result = GetJsonStr( 1 );
                            }
                            else
                            {
                                if ( _IsCheck == "1" )
                                {
                                    _Result = GetJsonStr( 0, 0, "" );
                                }
                                else
                                {
                                    string _VcChar = Text.GetVcChar();

                                    string _VcCharCookieKey = "_vcWxCodeChar" + DateTime.Now.ToString( "ssffffff" );
                                    //CouchBaseClient _Couch = new CouchBaseClient( 2 );
                                    //_Couch.SetObject( _VcCharCookieKey, _VcChar, 10 * 60 * 1000d );

                                    UtilityFun.SetCookie( _VcCharCookieKey, _VcChar, "", 10, false );

                                    //写入cookie
                                    UtilityFun.SetCookie( "_vcKey", couchBasekey, "", 10, false );
                                    string _EncrypedVcCharCookieKey = UtilityCryptography.AESEncrypt( _VcCharCookieKey );
                                    _Result = string.Format( "\"state\":{0},\"str\":\"{1}\",\"vcChar\":\"{2}\"", 0, _EncrypedVcCharCookieKey, _VcChar );
                                }
                            }
                        }
                        else
                        {
                            _Result = GetJsonStr( 2 );
                        }
                    }
                    catch ( Exception )
                    {
                        _Result = "\"state\":-2";
                    }
                    #endregion
                    break;
                #endregion

                #region 图形验证码的比较
                case "VcCompare":
                    try
                    {
                        string _Key = UtilityFun.GetCookie( "_vcKey", false );
                        if ( !string.IsNullOrEmpty( _Key ) )
                        {
                            int reqTimes = 1;//RequestLimit.CheckVcCodeRequestTimes( _Key );
                            if ( reqTimes >= 2 )
                            {
                                _Result = GetJsonStr( 1, 0 );//超出请求次数限制
                            }
                            else
                            {
                                double x = double.Parse( UtilityFun.ReqStrSQL( "x", "0" ) );
                                double y = double.Parse( UtilityFun.ReqStrSQL( "y", "0" ) );
                                int fontSize = UtilityFun.ToInt32( ConfigurationManager.AppSettings["VcCodeFontSize"] );
                                fontSize = fontSize <= 0 ? 1 : fontSize;
                                Position p = new Position
                                {
                                    XDis = x,
                                    YDis = y
                                };
                                bool confirmResult = new VcBLL().ConfirmVcCodeAuth( p, fontSize );
                                //using ( VcBLL _IBL = new VcBLL() )
                                //{
                                //    confirmResult = new VcBLL().ConfirmVcCodeAuth( p, fontSize );
                                //}
                                //if ( confirmResult )
                                //{
                                //    RequestLimit.UpdateVcCodeRequestTimes( _Key, 0 );
                                //}
                                //else
                                //{
                                //    RequestLimit.UpdateVcCodeRequestTimes( _Key, BLLVar.VcCodeOverTimes * 60 * 1000d );
                                //}
                                //AESEncrypt(_Key),对md5字符串再次进行AES加密，没有问题。
                                _Result = confirmResult ?
                                    GetJsonStr( 0, 0, UtilityCryptography.AESEncrypt( _Key ) )
                                    : GetJsonStr( 1, 1, "" );//点击位置错误
                            }

                        }
                        else
                        {
                            _Result = GetJsonStr( 2 );
                        }
                    }
                    catch
                    {
                        _Result = GetJsonStr( -2 );
                    }
                    break;
                #endregion
            }

            //设置过小压缩后反会增大
            if ( _Result.Length < 120 )
            {
                UtilityFun.SetUnZip();
            }

            if ( _IsHttp )
            {
                return _Result;
            }

            return _IsGet ? Exit( _Result ) : "{" + _Result + "}";
        }
        /// <summary>
        /// 获取Code值
        /// </summary>
        /// <param name="Result">Code字符串</param>
        /// <returns></returns>
        private string GetCodeResult( string Result )
        {
            string[] _Result = Result.Split( ':' );
            string _TmpStr = string.Empty;
            if ( _Result.Length > 1 )
            {
                _TmpStr = _Result[1];
            }
            return _TmpStr;
        }
        /// <summary>
        /// 输出json结果
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        private string Exit( string json )
        {
            string _CallFun = UtilityFun.ReqStrSQL( "fun", "" );
            if ( _CallFun == "" )
            {
                HttpContext.Current.Response.ContentType = "application/json";
                return "{" + json + "}";
            }
            else
            {
                return string.Format( "{0}({{{1}}})", _CallFun, json );
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 组合标准json返回值
        /// <summary>
        /// 组合标准json返回值, state
        /// </summary>
        /// <param name="state">处理状态:0成功，1失败，-3初始，-2异常，-1参数有误</param>
        /// <returns></returns>
        private string GetJsonStr( int state )
        {
            return string.Format( "\"state\":{0}", state );
        }
        /// <summary>
        /// 组合标准json返回值, state,num
        /// </summary>
        /// <param name="state">处理状态:0成功，1失败，-3初始，-2异常，-1参数有误</param>
        /// <param name="num">错误特征码，当state为1时有效</param>
        /// <returns></returns>
        private string GetJsonStr( int state, int num )
        {
            return string.Format( "\"state\":{0}, \"num\":{1}", state, num );
        }
        /// <summary>
        /// 组合标准json返回值, state,str
        /// </summary>
        /// <param name="state">处理状态:0成功，1失败，-3初始，-2异常，-1参数有误</param>
        /// <param name="str">附加返回内容，无则为null</param>
        /// <returns></returns>
        private string GetJsonStr( int state, string str )
        {
            return string.Format( "\"state\":{0}, \"str\":\"{1}\"", state, str );
        }
        /// <summary>
        /// 组合标准json返回值, state,num,str
        /// </summary>
        /// <param name="state">处理状态:0成功，1失败，-3初始，-2异常，-1参数有误</param>
        /// <param name="num">错误特征码，当state为1时有效</param>
        /// <param name="str">附加返回内容，无则为null</param>
        /// <returns></returns>
        private string GetJsonStr( int state, int num, string str )
        {
            return string.Format( "\"state\":{0}, \"num\":{1},\"str\":\"{2}\"", state, num, str );
        }
        #endregion

    }
}

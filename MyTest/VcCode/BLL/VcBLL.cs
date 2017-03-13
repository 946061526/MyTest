using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTest.VcCode.util;
using NSYGShop;
using System.Web.UI;

namespace MyTest.VcCode.BLL
{
    public class VcBLL
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="authCode">验证码</param>
        /// <returns></returns>
        public bool SetAuth( string authCode )
        {
            bool _Result = true;
            try
            {
                string _Key = "login" + DateTime.Now.ToString( "ssffffff" );
                //CouchBaseClient _Couch = new CouchBaseClient();
                //_Result = _Couch.SetObject( _Key, authCode, 600 * 1000d );

                UtilityFun.SetCookie( _Key, authCode, "", 10, false );

                UtilityFun.SetCookie( "_login", _Key, "", 10, false );
                //_Couch = null;
            }
            catch ( Exception ex )
            {
                _Result = false;
                UtilityFile.AddLogMsg( "BLLUser.SetAuth抛出异常：" + ex.Message );
            }
            return _Result;
        }


        /// <summary>
        /// 更新图形验证码
        /// </summary>
        /// <returns></returns>
        public bool SetVcCodeAuth( Position vcCode )
        {
            bool _Result = true;
            try
            {
                string _Key = "vcWxCode" + DateTime.Now.ToString( "ssffffff" );
                string _Value = vcCode.ChineseChar + "," + vcCode.XDis + "," + vcCode.YDis;
                //CouchBaseClient _Couch = new CouchBaseClient();
                //_Result = _Couch.SetObject( _Key, _Value, 10 * 60 * 1000d );
                HttpContext.Current.Session[_Key] = _Value;
                UtilityFun.SetCookie( "_vcWxCode", _Key, "", 10, true );
            }
            catch ( Exception ex )
            {
                _Result = false;
                UtilityFile.AddLogMsg( "BLLUser.SetVcCodeAuth抛出异常：" + ex.Message );
            }
            return _Result;
        }

        /// <summary>
        /// 比对验证码
        /// 正确之后失效
        /// </summary>
        /// <param name="authCode">提交的验证码</param>
        /// <returns></returns>
        public bool ConfirmAuth( string authCode )
        {
            bool _Result = false;
            try
            {
                string _Key = UtilityFun.GetCookie( "_login", false );
                if ( _Key != "" )
                {
                    //CouchBaseClient _Couch = new CouchBaseClient();
                    //string _OldCode = _Couch.GetObject<string>( _Key );
                    //if ( _OldCode != null && _OldCode.ToLower() == authCode.ToLower() )
                    //{
                    //    _Result = true;
                    //    _Couch.RemoveObject( _Key );
                    //    UtilityFun.DelCookie( "_login", "" );
                    //}
                    //else
                    //{
                    //    UtilityFile.AddLogErrMsg( "login", "key is " + _Key + ", submit code is " + authCode + ", org code is " + _OldCode + ", ip is " + UtilityFun.GetUserIP() );
                    //}
                    //_Couch = null;

                    string _OldCode = HttpContext.Current.Session[_Key].ToString();
                    if ( _OldCode != null && _OldCode.ToLower() == authCode.ToLower() )
                    {
                        _Result = true;
                        HttpContext.Current.Session[_Key] = null;
                        UtilityFun.DelCookie( "_login", "" );
                    }
                }
                else
                {
                    //考虑到部分用户可能不支持Cookies，在没有检测功能情况下，先让用户通过。
                    _Result = true;
                }
            }
            catch ( Exception ex )
            {
                _Result = false;
                UtilityFile.AddLogMsg( "BLLUser.ConfirmAuth抛出异常：" + ex.Message );
            }
            return _Result;
        }


        /// <summary>
        /// 对比验证信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool ConfirmVcStr( string str )
        {
            string _VcWxCode = UtilityFun.GetCookie( "_vcKey", false );
            str = UtilityCryptography.AESDecrypt( str );
            return string.Equals( _VcWxCode, str, StringComparison.CurrentCultureIgnoreCase );
        }

        public bool ConfirmVcCodeAuth( Position vcCode, int fontSize )
        {
            bool _Result = false;
            try
            {
                string _Key = UtilityFun.GetCookie( "_vcWxCode", true );
                if ( _Key != "" && fontSize > 0 )
                {
                    string _OldCodeStr = HttpContext.Current.Session[_Key].ToString();
                    if ( string.IsNullOrEmpty( _OldCodeStr ) )
                    {
                        UtilityFile.AddLogErrMsg( "_vcWxCode", "_OldCodeStr is null, CouchbaseKey code is " + _Key + ", ip is " + UtilityFun.GetUserIP() );
                    }
                    else
                    {
                        Position _OldCode = new Position();
                        string[] _Temp = _OldCodeStr.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
                        //_Temp肯定Length为3
                        _OldCode.XDis = int.Parse( _Temp[1] );
                        _OldCode.YDis = int.Parse( _Temp[2] );
                        _OldCode.ChineseChar = _Temp[0];// 汉字也需要比较

                        double dis = Position.GetDistanceBetweenPositions( vcCode, _OldCode );
                        if ( dis <= (double)fontSize / 2 )
                        {
                            _Result = true;
                            //这两个不要清空，因为在登录等业务里，残留的cookie的value值 和 _CouchBase中的value值还会被当成验证信息。 这两个等其自然失效
                            //_Couch.RemoveObject(_Key);
                            //UtilityFun.DelCookie("_vcWxCode", "");
                        }
                        else
                        {
                            UtilityFile.AddLogErrMsg( "_vcWxCode", "key is " + _Key + ", CouchbaseKey code is " + _Key + ", ip is " + UtilityFun.GetUserIP() );
                        }
                    }
                }
                else
                {
                    //考虑到部分用户可能不支持Cookies，在没有检测功能情况下，先让用户通过。
                    _Result = true;
                    UtilityFile.AddLogErrMsg( "_vcWxCode", "key is null, submit code is " + vcCode.ToString() + ", ip is " + UtilityFun.GetUserIP() );
                }
            }
            catch ( Exception ex )
            {
                _Result = false;
                UtilityFile.AddLogMsg( "BLLUser.ConfirmVcCodeAuth抛出异常：" + ex.Message );
            }
            return _Result;
        }
    }
}
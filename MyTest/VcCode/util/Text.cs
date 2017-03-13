using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MyTest.VcCode.util
{
    public class Text
    {
        public static string TxtPath = ConfigurationManager.AppSettings["txtPath"];


        public static List<string> GetCharListByChar( string selectedChar, int totalNum )
        {
            string rootPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string textFilePath = Path.Combine( rootPath, TxtPath );
            string text = File.ReadAllText( textFilePath );

            //MDLUpdateCouch _MDLUC = new MDLUpdateCouch( "weixin_vcchar", 7200 );//7200-720(120分钟)
            //CouchBaseClient _Couch = new CouchBaseClient();
            //string text = _Couch.GetCouchData<string>( _MDLUC, new CouchBaseClient.GetDataSourceDelegate<string>( GetVcCharStringData ), null );

            List<string> chineseChars = new List<string>();
            int txtLen = text.Length;
            while ( true )
            {
                int temp = VcRandom.GetRandomGuid( 0, txtLen );
                string t = text[temp].ToString();
                if ( !chineseChars.Contains( t ) )
                {
                    chineseChars.Add( t );
                }
                if ( chineseChars.Count == totalNum )
                {
                    break;
                }
            }
            return chineseChars;
        }



        private static string GetVcCharStringData( params object[] para )
        {
            string rootPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string textFilePath = Path.Combine( rootPath, TxtPath );
            //string text1 = @"E:\Proj\1yyg2\webWeixinUI\Vc\Resource\text.txt";
            string text = File.ReadAllText( textFilePath );
            return text;
        }

        /// <summary>
        /// 获取文本中的某一个字符
        /// 做了一个缓存
        /// </summary>
        /// <returns></returns>
        public static string GetVcChar()
        {
            //MDLUpdateCouch _MDLUC = new MDLUpdateCouch( "weixin_vcchar", 7200 );//7200-720(120分钟)
            //CouchBaseClient _Couch = new CouchBaseClient();
            //string text = _Couch.GetCouchData<string>( _MDLUC, new CouchBaseClient.GetDataSourceDelegate<string>( GetVcCharStringData ), null );
            string rootPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string textFilePath = Path.Combine( rootPath, TxtPath );
            string text = File.ReadAllText( textFilePath );

            var txtLen = text.Length;
            var temp = VcRandom.GetRandomGuid( 0, txtLen );
            var t = text[temp].ToString();
            return t;
        }
    }
}
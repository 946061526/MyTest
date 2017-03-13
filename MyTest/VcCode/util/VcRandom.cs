using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MyTest.VcCode.util
{
    public class VcRandom
    {

        /// <summary>
        /// [min,max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomGuid( int min, int max )
        {
        //卧槽 这语法……
        loop:
            int rel = GetRandomGuid();
            if ( rel >= min && rel <= max )
            {
                return rel;
            }
            goto loop;
        }


        public static int GetRandomGuid()
        {
            string str = Guid.NewGuid().ToString( "N" ).Substring( 0, 4 );//4
            int rel = 0;
            int.TryParse( str, NumberStyles.AllowHexSpecifier, null, out rel );
            return rel;
        }



        /// <summary>
        /// 随机产生一个true或false
        /// </summary>
        /// <returns></returns>
        public static bool GetRandomTrueOrFalse()
        {
            char ch = Guid.NewGuid().ToString( "N" ).Last();
            return ch % 2 == 0;
        }
    }
}
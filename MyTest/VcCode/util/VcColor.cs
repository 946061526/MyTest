using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MyTest.VcCode.util
{
    public class VcColor
    {
        public static IEnumerable<string> GetColorHexString( string strFromWebConfig )
        {
            return strFromWebConfig.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries ).ToList();
        }

        public static Color ConvertHexStringToColor( string hexStr )
        {
            Color color = Color.FromArgb( Convert.ToInt32( hexStr, 16 ) );
            Color color2 = Color.FromArgb( 255, color.R, color.G, color.B );
            return color2;
        }

        public static List<Color> GetColorListFromHexString( string hexColorString )
        {
            IEnumerable<string> colorStrList = GetColorHexString( hexColorString );
            List<Color> colorList = colorStrList.Select( ConvertHexStringToColor ).ToList();
            return colorList;
        }
    }
}
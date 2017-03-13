using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MyTest.VcCode.util
{
    public class Position
    {
        public string ChineseChar
        {
            get;
            set;
        }

        public double XDis
        {
            get;
            set;
        }

        public double YDis
        {
            get;
            set;
        }


        public Position()
        {
            ;
        }

        public Position( double x, double y )
        {
            this.XDis = x;
            this.YDis = y;
        }


        //勾股算弦
        public static double GetDistanceBetweenPositions( Position p1, Position p2 )
        {
            double bowstringLength = Math.Sqrt( Math.Abs( p1.XDis - p2.XDis ) * Math.Abs( p1.XDis - p2.XDis ) + Math.Abs( p1.YDis - p2.YDis ) * Math.Abs( p1.YDis - p2.YDis ) );
            return bowstringLength;
        }

        public override string ToString()
        {
            string result = string.Format( "{0},{1},{2}", ChineseChar, XDis, YDis );
            return result;
        }
    }

    public class Line
    {
        public Point start
        {
            get;
            set;
        }
        public Point end
        {
            get;
            set;
        }

        public Line( Point s, Point e )
        {
            start = s;
            end = e;
        }
    }
}
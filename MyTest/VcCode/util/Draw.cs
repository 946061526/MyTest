using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using Image = System.Drawing.Image;

namespace MyTest.VcCode.util
{
    public class Draw
    {
        public string VcCodeLimitType = ConfigurationManager.AppSettings["vcCodeLimitType"]; // m*n
        public bool IsShowCoordinateLine = bool.Parse( ConfigurationManager.AppSettings["isShowCoordinateLine"] );
        public string VcCodeCellLineColor = ConfigurationManager.AppSettings["vcCodeCellLineColor"];


        public string NormalBgFolderPath = ConfigurationManager.AppSettings["normalBgFolderPath"];
        public string AdvancedBgFolderPath = ConfigurationManager.AppSettings["advancedBgFolderPath"];
        public string TxtPath = ConfigurationManager.AppSettings["txtPath"];

        public bool IfCenterRedPointExist = bool.Parse( ConfigurationManager.AppSettings["ifCenterRedPointExist"] );

        public int FontSize = int.Parse( ConfigurationManager.AppSettings["vcCodeFontSize"] );
        public string FontFamilyStr = ConfigurationManager.AppSettings["vcCodeFontFamily"];
        public string CharColorList = ConfigurationManager.AppSettings["vcCodeHexColorList"];

        public bool IsInterferenceLineActive = bool.Parse( ConfigurationManager.AppSettings["isInterferenceLineActive"] );
        public int InterferenceLineNumber = int.Parse( ConfigurationManager.AppSettings["interferenceLineNumber"] );
        public string InterferenceLineColorList = ConfigurationManager.AppSettings["vcinterferenceLineColorList"];

        public bool IsImageNoiseActive = bool.Parse( ConfigurationManager.AppSettings["isImageNoiseActive"] );
        public int ImageNoiseNumber = int.Parse( ConfigurationManager.AppSettings["imageNoiseNumber"] );
        public string ImageNoiseColor = ConfigurationManager.AppSettings["imageNoiseColor"];
        public int ImageNoiseDiameter = int.Parse( ConfigurationManager.AppSettings["imageNoisePointDiameter"] );

        public static int vcCodeLimitTimes = int.Parse( ConfigurationManager.AppSettings["vcCodeLimitTimes"] );

        // 验证码字符旋转角度范围
        public string VcCodeCharRotateRange = ConfigurationManager.AppSettings["vcCodeCharRotateRange"];
        //验证码里的字符为空白的占比
        public string VcCodeCharSpaceRate = ConfigurationManager.AppSettings["vcCodeCharSpaceRate"];


        /// <summary>
        /// 传入参数，C# gdi绘图，并且得到文件流，显示在前台
        /// </summary>
        /// <param name="gdiWidth"></param>
        /// <param name="gdiHeight"></param>
        /// <param name="isAdvancedVcCode"></param>
        /// <param name="selectedChar"></param>
        /// <param name="fms"></param>
        /// <param name="positionList"></param>
        /// <param name="selectedPoint"></param>
        public void DrawVc( int gdiWidth, int gdiHeight, bool isAdvancedVcCode, string selectedChar, out MemoryStream fms, out List<Position> positionList, out Position selectedPoint )
        {

            //先处理图形形式 m*n
            int rows = int.Parse( VcCodeLimitType.Split( new[] { '*' }, StringSplitOptions.RemoveEmptyEntries )[0] );
            int cols = int.Parse( VcCodeLimitType.Split( new[] { '*' }, StringSplitOptions.RemoveEmptyEntries )[1] );

            string rootPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ;
            string folderPath = isAdvancedVcCode ? AdvancedBgFolderPath : NormalBgFolderPath;
            folderPath = Path.Combine( rootPath, folderPath );
            string[] files = Directory.GetFiles( folderPath );
            int randBgPic = VcRandom.GetRandomGuid( 0, files.Length - 1 );
            Bitmap image = (Bitmap)Image.FromFile( files[randBgPic] );
            image = ResizeImage( image, gdiWidth, gdiHeight, 1 );
            Graphics g = Graphics.FromImage( image );
            g.PageUnit = GraphicsUnit.Pixel;

            #region 画横线竖线

            //画横线竖线
            if ( IsShowCoordinateLine )
            {
                List<Line> lines = new List<Line>();
                for ( int i = 0; i < rows; i++ )
                {
                    Point hStart = new Point( 0, gdiHeight * i / rows );
                    Point hEnd = new Point( gdiWidth, gdiHeight * i / rows );
                    Line temp = new Line( hStart, hEnd );
                    lines.Add( temp );
                }
                for ( int j = 0; j < cols; j++ )
                {
                    Point vStart = new Point( gdiWidth * j / cols, 0 );
                    Point vEnd = new Point( gdiWidth * j / cols, gdiHeight );
                    Line temp = new Line( vStart, vEnd );
                    lines.Add( temp );
                }
                Color lineColor = VcColor.ConvertHexStringToColor( VcCodeCellLineColor );
                foreach ( Line t in lines )
                {
                    g.DrawLine( new Pen( lineColor ), t.start, t.end );
                }
            }

            #endregion

            #region 每个格子的中心点

            //每个格子的中心点
            List<Point> cellCenterPoints = new List<Point>();
            for ( int i = 0; i < rows; i++ )
            {
                for ( int j = 0; j < cols; j++ )
                {
                    double xTemp = ( 2 * j + 1 ) * gdiWidth / ( 2 * cols );
                    double yTemp = ( 2 * i + 1 ) * gdiHeight / ( 2 * rows );
                    Point pTemp = new Point( (int)xTemp, (int)yTemp );
                    cellCenterPoints.Add( pTemp );
                }
            }

            #endregion

            #region 得到待写入画板的不重复的字符序列，数目等于方格的数目

            string textFilePath = Path.Combine( rootPath, TxtPath );
            string txt = File.ReadAllText( textFilePath );
            List<string> chineseChars = new List<string>();
            int txtLen = txt.Length;
            while ( true )
            {
                int temp = VcRandom.GetRandomGuid( 0, txtLen );
                string t = txt[temp].ToString();
                if ( ( !chineseChars.Contains( t ) ) && ( t != selectedChar ) )
                {
                    chineseChars.Add( t );
                }
                if ( chineseChars.Count == rows * cols )
                {
                    break;
                }
            }

            #endregion

            #region 处理空白格百分比问题
            string[] spaceRate = VcCodeCharSpaceRate.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ); // 0.2  0.5
            double minSpaceRate = double.Parse( spaceRate[0] );
            double maxSpaceRate = double.Parse( spaceRate[1] );
            int minSpaceNum = (int)Math.Ceiling( rows * cols * minSpaceRate );
            int maxSpaceNum = (int)Math.Floor( rows * cols * maxSpaceRate );
            int randSpaceNum = VcRandom.GetRandomGuid( minSpaceNum, maxSpaceNum );
            List<int> existedIndexList = new List<int>();
            for ( int i = 0; i < randSpaceNum; i++ )
            {
                int t = 0;
            //注意goto
            loop:
                t = VcRandom.GetRandomGuid( 0, chineseChars.Count - 1 );
                if ( !existedIndexList.Contains( t ) )
                {
                    existedIndexList.Add( t );
                }
                else
                {
                    goto loop;
                }
                chineseChars[t] = " ";//中文的空格键
            }
            #endregion

            #region 巧妙处理选定字符的方法：从chineseChars中随机选择一个元素，被selectedChar替换即可
            int ranIndex = VcRandom.GetRandomGuid( 0, chineseChars.Count - 1 );
            chineseChars[ranIndex] = selectedChar;
            #endregion

            #region 干扰线
            Random rand = new Random();

            //干扰线颜色
            List<Color> interferenceColorList = VcColor.GetColorListFromHexString( InterferenceLineColorList );
            if ( IsInterferenceLineActive )
            {
                for ( int i = 0; i < InterferenceLineNumber; i++ )
                {
                    int x1 = rand.Next( image.Width );
                    int x2 = rand.Next( image.Width );
                    int y1 = rand.Next( image.Height );
                    int y2 = rand.Next( image.Height );
                    int colRan = rand.Next( 0, interferenceColorList.Count );
                    Brush brush = new SolidBrush( interferenceColorList[colRan] );
                    Pen pen = new Pen( brush, rand.Next( 1, 4 ) );
                    g.DrawLine( pen, x1, x2, y1, y2 );
                }
            }
            #endregion

            #region 噪点
            if ( IsImageNoiseActive )
            {
                Color noiseColor = VcColor.GetColorListFromHexString( ImageNoiseColor ).First();
                for ( int i = 0; i < ImageNoiseNumber; i++ )
                {
                    int x = rand.Next( image.Width );
                    int y = rand.Next( image.Height );
                    Brush b = new SolidBrush( noiseColor );
                    g.FillEllipse( b, x, y, ImageNoiseDiameter, ImageNoiseDiameter );
                }
            }
            #endregion

            #region 在格子里写字，字体、颜色随机，字号、旋转角度可配置

            //定义字体 
            string[] fontFamily = FontFamilyStr.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
            for ( int i = 0; i < fontFamily.Length; i++ )
            {
                fontFamily[i] = fontFamily[i].Trim();
            }
            int fontFamilyCount = fontFamily.Length;
            //定义字体颜色
            List<Color> charColorList = VcColor.GetColorListFromHexString( CharColorList );

            string[] rotateAngle = VcCodeCharRotateRange.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
            int minRotateAngle = int.Parse( rotateAngle[0] );
            int maxRotateAngle = int.Parse( rotateAngle[1] );

            for ( int i = 0; i < chineseChars.Count; i++ )
            {
                //随机颜色
                int randColor = VcRandom.GetRandomGuid( 0, charColorList.Count - 1 );
                Brush b = new SolidBrush( charColorList[randColor] );
                //随机字体
                string fontFamilyStr = fontFamily[VcRandom.GetRandomGuid( 0, fontFamilyCount - 1 )];
                Font f = new Font( fontFamilyStr, FontSize );
                //计算此字体占用长宽像素数
                SizeF size = g.MeasureString( chineseChars[i], f ); //这个函数非常重要，可以得出此种字体此种字号的字写在这个画板上占用的长宽像素
                Point nP = new Point( (int)( -size.Width / 2 ), (int)( -size.Height / 2 ) );
                //旋转
                g.TranslateTransform( cellCenterPoints[i].X, cellCenterPoints[i].Y );
                int flagDegree = VcRandom.GetRandomGuid( minRotateAngle, maxRotateAngle ) * ( VcRandom.GetRandomTrueOrFalse() ? 1 : -1 );
                g.RotateTransform( flagDegree );
                g.DrawString( chineseChars[i], f, b, nP );
                g.RotateTransform( ( -1 ) * flagDegree );
                g.TranslateTransform( -cellCenterPoints[i].X, -cellCenterPoints[i].Y );

                //画出一些便于定位的中心点，便于定位
                if ( IfCenterRedPointExist )
                {
                    g.DrawRectangle( new Pen( Color.Red ), cellCenterPoints[i].X, cellCenterPoints[i].Y, 1, 1 );
                }
            }

            #endregion


            //输出到浏览器
            MemoryStream ms = new MemoryStream();
            image.Save( ms, System.Drawing.Imaging.ImageFormat.Png );
            g.Dispose();
            image.Dispose();

            fms = ms;
            if ( chineseChars.Count == cellCenterPoints.Count )
            {
                positionList = new List<Position>();
                positionList.AddRange( chineseChars.Select( ( t, i ) => new Position()
                {
                    XDis = cellCenterPoints[i].X,
                    YDis = cellCenterPoints[i].Y,
                    ChineseChar = t
                } ) );

                selectedPoint = new Position
                {
                    XDis = cellCenterPoints[ranIndex].X,
                    YDis = cellCenterPoints[ranIndex].Y,
                    ChineseChar = selectedChar
                };
            }
            else
            {
                positionList = null;
                selectedPoint = null;
            }
        }

        public static Bitmap ResizeImage( Bitmap bmp, int newW, int newH, int Mode )
        {
            try
            {
                Bitmap b = new Bitmap( newW, newH );
                Graphics g = Graphics.FromImage( b );
                // 插值算法的质量   
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage( bmp, new Rectangle( 0, 0, newW, newH ), new Rectangle( 0, 0, bmp.Width, bmp.Height ), GraphicsUnit.Pixel );
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
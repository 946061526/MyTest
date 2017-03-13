using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTest
{
    public partial class FileStreamTest : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            // Specify a file to read from and to create.
            string pathSource = @"d:\tests\source.txt";
            string pathNew = @"d:\tests\newfile.txt";

            try
            {

                using ( FileStream fsSource = new FileStream( pathSource,
                    FileMode.Open, FileAccess.Read ) )
                {

                    // Read the source file into a byte array.
                    byte[] bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while ( numBytesToRead > 0 )
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read( bytes, numBytesRead, numBytesToRead );

                        // Break when the end of the file is reached.
                        if ( n == 0 )
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = bytes.Length;

                    // Write the byte array to the other FileStream.
                    using ( FileStream fsNew = new FileStream( pathNew,
                        FileMode.Create, FileAccess.Write ) )
                    {
                        fsNew.Write( bytes, 0, numBytesToRead );
                    }

                    //////
                    byte[] b = new byte[10];
                    fsSource.Read( b, 0, 10 );

                }
            }
            catch ( FileNotFoundException ioEx )
            {
                Console.WriteLine( ioEx.Message );
            }
        }
    }
}